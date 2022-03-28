using UnityEngine;

public class PlayerManagerMultiplayer : PlayerManager
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject localPlayerInstance;

    void Awake()
    {
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManagerMultiplayer.localPlayerInstance = gameObject;
        }

        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        SpriteRenderer[] playerSprites = GetComponentsInChildren<SpriteRenderer>();
        playerSprites[0].enabled = photonView.IsMine;
        playerSprites[1].enabled = !photonView.IsMine;

        base.Start();

        bird.Sleep();
    }
}
