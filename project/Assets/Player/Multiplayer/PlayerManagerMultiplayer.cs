using System.Collections;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class PlayerManagerMultiplayer : PlayerManager
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject localPlayerInstance;

    [HideInInspector]
    public bool ready = false;

    protected override void Awake()
    {
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManagerMultiplayer.localPlayerInstance = gameObject;
        }

        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);

        if (!PlayerList.players.Contains(gameObject))
        {
            PlayerList.players.Add(gameObject);
        }
    }

    protected override void Start()
    {
        SpriteRenderer[] playerSprites = GetComponentsInChildren<SpriteRenderer>();
        playerSprites[0].enabled = photonView.IsMine;
        playerSprites[1].enabled = !photonView.IsMine;

        base.Start();

        bird.Sleep();

        ready = false;

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        StartCoroutine("WaitForReadyUp");
    }

    void OnEnable()
    {
        EventManager.OnGameOver += FallAsleep;
    }

    void OnDisable()
    {
        EventManager.OnGameOver -= FallAsleep;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        EventManager.OnGameOver -= FallAsleep;
    }

    [PunRPC]
    private void SetReady(bool ready)
    {
        this.ready = ready;
    }

    private IEnumerator WaitForReadyUp()
    {
        // wait for player to input first movement to start the game
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.UpArrow));

        photonView.RPC("SetReady", RpcTarget.All, true);
    }

    public override void IncrementScore()
    {
        if (photonView.IsMine) playerScore++;
    }

    /// <summary>
    /// Method <c>Kill</c> Kills the player, invoking OnPlayerDeath event
    /// </summary>
    public override void Kill()
    {
        if (gameObject.Equals(PlayerManagerMultiplayer.localPlayerInstance))
        {
            // sleeps the rigidbody of the player to disable physics simulation
            photonView.RPC("FallAsleep", RpcTarget.All);

            // disables player movement so the player can't continue playing after losing
            gameObject.GetComponent<PlayerMovement>().enabled = false;

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventManager.OnPlayerDeathPhotonEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    [PunRPC]
    private void FallAsleep()
    {
        bird.Sleep();
    }
}
