using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerManagerMultiplayer : PlayerManager
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject localPlayerInstance;

    [HideInInspector]
    public bool ready = false;

    [PunRPC]
    private void SetReady(bool ready)
    {
        this.ready = ready;
    }

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

    private IEnumerator WaitForReadyUp()
    {
        // wait for player to input first movement to start the game
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.UpArrow));

        photonView.RPC("SetReady", RpcTarget.All, true);
    }
}
