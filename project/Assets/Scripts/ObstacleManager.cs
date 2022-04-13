using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObstacleManager : MonoBehaviourPun
{
    // obstacle rigidbody
    protected Rigidbody2D obstacle;

    // Wrappers
    protected ToolsWrapper tools;
    protected Rigidbody2DWrapper obstacleWrapper;

    void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();

        InitWrappers();
    }

    protected virtual void OnEnable()
    {
        EventManager.OnGameOver += Sleep;
    }

    protected virtual void OnDisable()
    {
        EventManager.OnGameOver -= Sleep;
    }

    protected virtual void OnDestroy()
    {
        EventManager.OnGameOver -= Sleep;
    }

    protected virtual void OnTriggerEnter2D(Collider2D intruderCollider)
    {
        GameObject intruder = intruderCollider.gameObject;

        // act depending on the other collider:
        if (intruder.layer == LayerMask.NameToLayer("Reset Checkpoint")) // loop the obstacle to the other side of the game
        {
            Cycle();
        }
        else if (intruder.layer == LayerMask.NameToLayer("Reposition Checkpoint")) // change the obstacle's height
        {
            RepositionCheckpointWrapper repositionCheckpoint = GetRepositionCheckpointWrapperFromGameObject(intruder);

            float newHeight = tools.LimitedRandomVariance(repositionCheckpoint.GetLastObstacleHeight(),
                                                            Consts.MIN_GAP_HEIGHT,
                                                            Consts.MAX_GAP_HEIGHT,
                                                            Consts.MAX_ABS_VARIANCE);

            RepositionY(newHeight);

            repositionCheckpoint.SetLastObstacleHeight(newHeight);
        }
        else if (intruder.layer == LayerMask.NameToLayer("Player")) // increment the score
        {
            EventManager.OnObstacleClear?.Invoke(new GameObjectWrapper(intruder));
        }
    }

    private void InitWrappers()
    {
        if (tools == null) { tools = new ToolsWrapper(); }
        obstacleWrapper = new Rigidbody2DWrapper(obstacle);
    }

    /// <summary>
    /// Method <c>WakeUp</c> Wakes up the rigidbody of the obstacle to allow physics simulation
    /// </summary>
    public virtual void WakeUp()
    {
        if (obstacleWrapper.IsSleeping())
        {
            obstacleWrapper.WakeUp();

            // sets the obstacles velocity so it starts moving, allowing the player to clear it
            obstacleWrapper.velocity = new Vector2(-Consts.GAME_X_SCROLLING_SPEED, 0);
        }
    }

    public void Sleep()
    {
        obstacleWrapper.Sleep();
    }

    // cycles the obstacle to the other side of game, effectively respawning it
    public virtual void Cycle()
    {
        obstacleWrapper.position = new Vector2(14f, obstacleWrapper.position.y);
    }

    // repositions the obstacle in the y axis
    public virtual void RepositionY(float value)
    {
        obstacleWrapper.position = new Vector2(obstacleWrapper.position.x, value);
    }

    protected virtual RepositionCheckpointWrapper GetRepositionCheckpointWrapperFromGameObject(GameObject collider)
    {
        return new RepositionCheckpointWrapper(collider.GetComponent<RepositionCheckpoint>());
    }
}
