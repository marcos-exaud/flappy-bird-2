using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class UIManagerTests
{
    private UIManager manager;
    private TextMeshProUGUI scoreboard;
    private GameObjectWrapper gameOverUI;
    private GameObjectWrapper multiplayerMenuUI;
    private GameObjectWrapper defaultMenuUI;
    private GameObjectWrapper controlPanel;
    private GameObjectWrapper progressLabel;

    [SetUp]
    public void Setup()
    {
        InitComponents();

        //Variable Reflexion attribution
        ReflectionUtils.SetValue(manager, "scoreboard", scoreboard);
        ReflectionUtils.SetValue(manager, "gameOverUIWrapper", gameOverUI);
        ReflectionUtils.SetValue(manager, "multiplayerMenuUIWrapper", multiplayerMenuUI);
        ReflectionUtils.SetValue(manager, "defaultMenuUIWrapper", defaultMenuUI);
        ReflectionUtils.SetValue(manager, "controlPanelWrapper", controlPanel);
        ReflectionUtils.SetValue(manager, "progressLabelWrapper", progressLabel);
    }

    public void InitComponents()
    {
        // manager = new ConnectionManager();
        manager = Substitute.ForPartsOf<UIManager>();

        // Substitutes
        scoreboard = Substitute.For<TextMeshProUGUI>();
        gameOverUI = Substitute.For<GameObjectWrapper>();
        multiplayerMenuUI = Substitute.For<GameObjectWrapper>();
        defaultMenuUI = Substitute.For<GameObjectWrapper>();
        controlPanel = Substitute.For<GameObjectWrapper>();
        progressLabel = Substitute.For<GameObjectWrapper>();
    }

    [Test]
    public void _1_UpdateScoreboard()
    {
        //Arrange
        GameObjectWrapper player = Substitute.For<GameObjectWrapper>();
        PlayerManager playerManager = Substitute.For<PlayerManager>();
        playerManager.GetPlayerScore().Returns<int>(17);
        player.GetComponent<PlayerManager>().Returns<PlayerManager>(playerManager);

        //Act
        ReflectionUtils.Invoke(manager, "UpdateScoreboard", new object[] { player });

        //Assert
        Assert.AreEqual("17", ReflectionUtils.GetValue<TextMeshProUGUI>(manager, "scoreboard").text);
    }

    [Test]
    public void _2_DisplayGameOverUI()
    {
        //Arrange
        gameOverUI.When(x => x.SetActive(Arg.Any<bool>())).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "DisplayGameOverUI");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(gameOverUI, "SetActive", new object[] { true });
    }

    [Test]
    public void _3_ToggleMainMenuMultiplayerUI()
    {
        //Arrange
        bool testBool = true;
        multiplayerMenuUI.activeSelf.Returns<bool>(testBool);
        multiplayerMenuUI.When(x => x.SetActive(Arg.Any<bool>())).DoNotCallBase();
        defaultMenuUI.When(x => x.SetActive(Arg.Any<bool>())).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "ToggleMainMenuMultiplayerUI");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(multiplayerMenuUI, "SetActive", new object[] { !testBool });
        ReflectionUtils.AssertMethodIsCalled(defaultMenuUI, "SetActive", new object[] { testBool });
    }

    [Test]
    public void _4_ToggleMainMenuMultiplayerProgressUI()
    {
        //Arrange
        bool testBool = true;
        progressLabel.activeSelf.Returns<bool>(testBool);
        progressLabel.When(x => x.SetActive(Arg.Any<bool>())).DoNotCallBase();
        controlPanel.When(x => x.SetActive(Arg.Any<bool>())).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "ToggleMainMenuMultiplayerProgressUI");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(progressLabel, "SetActive", new object[] { !testBool });
        ReflectionUtils.AssertMethodIsCalled(controlPanel, "SetActive", new object[] { testBool });
    }
}
