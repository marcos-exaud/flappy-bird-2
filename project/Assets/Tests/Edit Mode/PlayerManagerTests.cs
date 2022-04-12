using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerManagerTests
{
    private PlayerManager manager;
    private Rigidbody2DWrapper bird;
    private GameObjectWrapper gameObject;

    [SetUp]
    public void Setup()
    {
        InitComponents();

        //Variable Reflexion attribution
        ReflectionUtils.SetValue(manager, "birdWrapper", bird);
        ReflectionUtils.SetValue(manager, "gameObjectWrapper", gameObject);
    }

    public void InitComponents()
    {
        manager = Substitute.ForPartsOf<PlayerManager>();

        // Substitutes
        bird = Substitute.For<Rigidbody2DWrapper>();
        gameObject = Substitute.For<GameObjectWrapper>();
    }

    [Test]
    public void _1_1_Test_WakeUp_player_is_asleep()
    {
        //Arrange
        bird.When(x => x.IsSleeping()).DoNotCallBase();
        bird.IsSleeping().Returns(true);

        bird.When(x => x.WakeUp()).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "WakeUp");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(bird, "WakeUp");
    }

    [Test]
    public void _1_2_Test_WakeUp_player_is_awake()
    {
        //Arrange
        bird.When(x => x.IsSleeping()).DoNotCallBase();
        bird.IsSleeping().Returns(false);

        //Act
        ReflectionUtils.Invoke(manager, "WakeUp");

        //Assert
        ReflectionUtils.AssertMethodIsNotCalled(bird, "WakeUp");
    }

    [Test]
    public void _2_Test_IncrementScore()
    {
        //Arrange
        ReflectionUtils.SetValue(manager, "playerScore", 3);

        //Act
        ReflectionUtils.Invoke(manager, "IncrementScore");

        //Assert
        Assert.AreEqual(true, ReflectionUtils.GetValue<int>(manager, "playerScore") == 4);
    }

    [Test]
    public void _3_Test_Kill()
    {
        //Arrange
        GameObject go = new GameObject();
        PlayerMovement playerMovement = go.AddComponent<PlayerMovement>();
        playerMovement.enabled = true;

        gameObject.When(x => x.GetComponent<PlayerMovement>()).DoNotCallBase();
        gameObject.GetComponent<PlayerMovement>().Returns(playerMovement);

        bool eventRaised = false;
        System.Action EventRaised = new System.Action(() => { eventRaised = true; });
        EventManager.OnPlayerDeath += EventRaised;

        //Act
        ReflectionUtils.Invoke(manager, "Kill");
        EventManager.OnPlayerDeath -= EventRaised;

        //Assert
        Assert.AreEqual(false, playerMovement.enabled);
        Assert.AreEqual(true, eventRaised);
    }

    [Test]
    public void _4_Test_GetPlayerScore()
    {
        //Arrange
        ReflectionUtils.SetValue(manager, "playerScore", 3);

        //Act
        int score = (int)ReflectionUtils.Invoke(manager, "GetPlayerScore");

        //Assert
        Assert.AreEqual(true, score == 3);
    }
}
