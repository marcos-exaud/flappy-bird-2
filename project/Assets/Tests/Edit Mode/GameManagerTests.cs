using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTests
{
    private GameManager manager;
    private List<GameObject> obstacles;
    private GameObject uiManager;
    private InputWrapper inputWrapper;

    [SetUp]
    public void Setup()
    {
        InitComponents();

        //Variable Reflexion attribution
        //ReflectionUtils.SetValue(manager, "uiManager", uiManager);
        ReflectionUtils.SetValue(manager, "inputWrapper", inputWrapper);
        //ReflectionUtils.SetValue(manager, "punWrapper", pun);
    }

    public void InitComponents()
    {
        // manager = new ConnectionManager();
        manager = Substitute.ForPartsOf<GameManager>();

        // Substitutes
        //uiManager = Substitute.For<GameObject>();
        inputWrapper = Substitute.For<InputWrapper>();
        //pun = Substitute.For<PhotonNetworkWrapper>();
    }

    [Test]
    public void _1_1_Test_CheckForGameOver_all_players_dead()
    {
        //Arrange
        PlayerManager playerManager = Substitute.For<PlayerManager>();
        GameObjectWrapper player = Substitute.For<GameObjectWrapper>();

        playerManager.PlayerIsAlive().Returns(false);
        player.GetComponent<PlayerManager>().Returns(playerManager);

        PlayerList.players.Add(player);

        bool eventRaised = false;
        System.Action EventRaised = new System.Action(() => { eventRaised = true; });
        EventManager.OnGameOver += EventRaised;

        //Act
        ReflectionUtils.Invoke(manager, "CheckForGameOver");
        EventManager.OnGameOver -= EventRaised;

        //Assert
        Assert.AreEqual(true, eventRaised);
    }

    [Test]
    public void _1_2_Test_CheckForGameOver_not_all_players_dead()
    {
        //Arrange
        PlayerManager playerManager = Substitute.For<PlayerManager>();
        GameObjectWrapper player = Substitute.For<GameObjectWrapper>();

        playerManager.PlayerIsAlive().Returns(true);
        player.GetComponent<PlayerManager>().Returns(playerManager);

        PlayerList.players.Add(player);

        bool eventRaised = false;
        System.Action EventRaised = new System.Action(() => { eventRaised = true; });
        EventManager.OnGameOver += EventRaised;

        //Act
        ReflectionUtils.Invoke(manager, "CheckForGameOver");
        EventManager.OnGameOver -= EventRaised;

        //Assert
        Assert.AreEqual(false, eventRaised);
    }
}
