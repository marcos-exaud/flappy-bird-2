using System.Collections;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManagerMultiplayer : PlayerManager, IPunObservable
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject localPlayerInstance;

    [HideInInspector]
    public bool ready = false;

    protected override void Awake()
    {
        InitWrappersAwake();
        
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManagerMultiplayer.localPlayerInstance = gameObject;
        }

        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);

        if (!PlayerList.players.Contains(gameObjectWrapper))
        {
            PlayerList.players.Add(gameObjectWrapper);
        }
    }

    protected override void Start()
    {
        SpriteRenderer[] playerSprites = gameObjectWrapper.GetComponentsInChildren<SpriteRenderer>();
        playerSprites[0].enabled = photonView.IsMine;
        playerSprites[1].enabled = !photonView.IsMine;

        base.Start();

        bird.Sleep();

        ready = false;

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            bird.isKinematic = true;
            return;
        }

        StartCoroutine("WaitForReadyUp");
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    public void OnPhotonEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch (eventCode)
        {
            case EventManager.OnGameOverPhotonEventCode:
                FallAsleep();
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(PlayerIsAlive());
            stream.SendNext(gameObjectWrapper.GetComponent<PlayerMovement>().enabled);
        }
        else
        {
            // Network player, receive data
            bool playerIsAlive = (bool)stream.ReceiveNext();
            if (!playerIsAlive && PlayerIsAlive()) FallAsleep();
            gameObjectWrapper.GetComponent<PlayerMovement>().enabled = (bool)stream.ReceiveNext();
        }
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

        EventManager.OnPlayerReadyUp?.Invoke();

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
        if (gameObject.Equals(PlayerManagerMultiplayer.localPlayerInstance) && MultiplayerGameManager.gameIsRunning)
        {
            Hashtable hash = new Hashtable();
            hash.Add("Score", playerScore);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            DestroyPlayerGameObject();

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent(EventManager.OnPlayerDeathPhotonEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    [PunRPC]
    public void FallAsleep()
    {
        bird.Sleep();
    }

    [PunRPC]
    public void DestroyPlayerGameObject()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }

    [PunRPC]
    public void DisplayNickname()
    {
        TextMeshPro nicknameLabel = GetComponentInChildren<TextMeshPro>();
        nicknameLabel.SetText(photonView.Owner.NickName);
    }
}
