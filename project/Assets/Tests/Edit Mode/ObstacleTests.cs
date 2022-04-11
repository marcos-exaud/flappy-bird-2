using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class ObstacleTests
{
    private Obstacle obstacle;

    [SetUp]
    public void Setup()
    {
        InitComponents();

        //Variable Reflexion attribution
    }

    public void InitComponents()
    {
        obstacle = Substitute.ForPartsOf<Obstacle>();

        // Substitutes
    }

    [Test]
    public void _1_1_Test_OnTriggerEnter2D_other_is_player()
    {
        //Arrange
        GameObject player = new GameObject();
        Collider2D collider = player.AddComponent<BoxCollider2D>();
        player.layer = LayerMask.NameToLayer("Player");

        PlayerManagerWrapper playerManager = Substitute.For<PlayerManagerWrapper>();
        playerManager.When(x => x.Kill()).DoNotCallBase();

        MethodInfo getPlayerManagerWrapperFromGameObjectMethodInfo = ReflectionUtils.GetMethod(obstacle, "GetPlayerManagerWrapperFromGameObject");
        getPlayerManagerWrapperFromGameObjectMethodInfo.Invoke(obstacle, new object[] { player }).Returns(playerManager);
        obstacle.When(x => getPlayerManagerWrapperFromGameObjectMethodInfo.Invoke(x, new object[] { player })).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(obstacle, "OnTriggerEnter2D", new object[] { collider });

        //Assert
        ReflectionUtils.AssertMethodIsCalled(playerManager, "Kill");
    }

    [Test]
    public void _1_2_Test_OnTriggerEnter2D_other_is_not_player()
    {
        //Arrange
        GameObject player = new GameObject();
        Collider2D collider = player.AddComponent<BoxCollider2D>();
        player.layer = LayerMask.NameToLayer("Default");

        //Act
        ReflectionUtils.Invoke(obstacle, "OnTriggerEnter2D", new object[] { collider });

        //Assert
        ReflectionUtils.AssertMethodIsNotCalled(obstacle, "GetPlayerManagerWrapperFromGameObject", new object[] { Arg.Any<GameObject>() });
    }
}
