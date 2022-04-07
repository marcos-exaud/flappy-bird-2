using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class ConnectionManagerTests
{
    private ConnectionManager manager;
    private GameObjectWrapper uiManager;
    private PhotonNetworkWrapper pun;
    private bool isConnecting;
    private byte maxPlayersPerRoom;
    private string gameVersion;

    [SetUp]
    public void Setup()
    {
        InitComponents();

        //Variable Reflexion attribution
        ReflectionUtils.SetValue(manager, "uiManagerWrapper", uiManager);
        ReflectionUtils.SetValue(manager, "punWrapper", pun);
        /*ReflectionUtils.SetValue(manager, "isConnecting", isConnecting);
        ReflectionUtils.SetValue(manager, "maxPlayersPerRoom", maxPlayersPerRoom);
        ReflectionUtils.SetValue(manager, "gameVersion", gameVersion);*/
    }

    public void InitComponents()
    {
        // manager = new ConnectionManager();
        manager = Substitute.ForPartsOf<ConnectionManager>();

        // Substitutes
        uiManager = Substitute.For<GameObjectWrapper>();
        pun = Substitute.For<PhotonNetworkWrapper>();
    }

    [Test]
    public void _1_1_Test_Connect_is_connceted_to_photon_network()
    {
        //Arrange
        UIManager uiManagerMockComponent = Substitute.For<UIManager>();
        uiManagerMockComponent.When(x => x.ToggleMainMenuMultiplayerProgressUI()).DoNotCallBase();
        uiManager.GetComponent<UIManager>().Returns<UIManager>(uiManagerMockComponent);

        pun.IsConnected.Returns<bool>(true);
        pun.When(x => x.JoinRandomRoom()).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "Connect");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(pun, "JoinRandomRoom");
    }

    [Test]
    public void _1_2_Test_Connect_is_not_connceted_to_photon_network()
    {
        //Arrange
        ReflectionUtils.SetValue(manager, "gameVersion", "2");

        UIManager uiManagerMockComponent = Substitute.For<UIManager>();
        uiManagerMockComponent.When(x => x.ToggleMainMenuMultiplayerProgressUI()).DoNotCallBase();
        uiManager.GetComponent<UIManager>().Returns<UIManager>(uiManagerMockComponent);

        pun.IsConnected.Returns<bool>(false);
        pun.ConnectUsingSettings().Returns<bool>(true);
        pun.When(x => x.ConnectUsingSettings()).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "Connect");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(pun, "ConnectUsingSettings");
        Assert.AreEqual(true, ReflectionUtils.GetValue<bool>(manager, "isConnecting"));
        Assert.AreEqual("2", ReflectionUtils.GetValue<PhotonNetworkWrapper>(manager, "punWrapper").GameVersion);
    }

    [Test]
    public void _2_1_Test_OnConnectedToMaster_is_connecting()
    {
        //Arrange
        ReflectionUtils.SetValue(manager, "isConnecting", true);

        pun.When(x => x.JoinRandomRoom()).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "OnConnectedToMaster");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(pun, "JoinRandomRoom");
        Assert.AreEqual(false, ReflectionUtils.GetValue<bool>(manager, "isConnecting"));
    }

    [Test]
    public void _2_2_Test_OnConnectedToMaster_is_not_connecting()
    {
        //Arrange
        ReflectionUtils.SetValue(manager, "isConnecting", false);

        //Act
        ReflectionUtils.Invoke(manager, "OnConnectedToMaster");

        //Assert
        ReflectionUtils.AssertMethodIsNotCalled(pun, "JoinRandomRoom");
    }

    [Test]
    public void _3_Test_OnDisconnected()
    {
        //Arrange
        ReflectionUtils.SetValue(manager, "isConnecting", true);

        UIManager uiManagerMockComponent = Substitute.For<UIManager>();
        uiManagerMockComponent.When(x => x.ToggleMainMenuMultiplayerProgressUI()).DoNotCallBase();
        uiManager.GetComponent<UIManager>().Returns<UIManager>(uiManagerMockComponent);

        //Act
        ReflectionUtils.Invoke(manager, "OnDisconnected", new object[] { null });

        //Assert
        Assert.AreEqual(false, ReflectionUtils.GetValue<bool>(manager, "isConnecting"));
    }

    [Test]
    public void _4_1_Test_OnJoinedRoom_first_player_in_room()
    {
        //Arrange
        pun.When(x => x.LoadLevel(Arg.Any<int>())).DoNotCallBase();
        manager.When(x => x.CurrentRoomPlayerCount()).DoNotCallBase();
        manager.CurrentRoomPlayerCount().Returns<byte>(1);

        //Act
        ReflectionUtils.Invoke(manager, "OnJoinedRoom");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(pun, "LoadLevel", new object[] { Consts.MULTIPLAYER_GAME_SCENE_1P });
    }

    [Test]
    public void _4_2_Test_OnJoinedRoom_is_not_first_player_in_room()
    {
        //Arrange
        manager.When(x => x.CurrentRoomPlayerCount()).DoNotCallBase();
        manager.CurrentRoomPlayerCount().Returns<byte>(2);

        //Act
        ReflectionUtils.Invoke(manager, "OnJoinedRoom");

        //Assert
        ReflectionUtils.AssertMethodIsNotCalled(pun, "LoadLevel", new object[] { Arg.Any<int>() });
    }
}
