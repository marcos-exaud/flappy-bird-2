using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class ObstacleManagerTests
{
    private ObstacleManager manager;
    private ToolsWrapper tools;
    private Rigidbody2DWrapper obstacle;

    [SetUp]
    public void Setup()
    {
        InitComponents();

        //Variable Reflexion attribution
        ReflectionUtils.SetValue(manager, "tools", tools);
        ReflectionUtils.SetValue(manager, "obstacleWrapper", obstacle);
    }

    public void InitComponents()
    {
        manager = Substitute.ForPartsOf<ObstacleManager>();

        // Substitutes
        tools = Substitute.For<ToolsWrapper>();
        obstacle = Substitute.For<Rigidbody2DWrapper>();
    }

    [Test]
    public void _1_1_Test_OnTriggerEnter2D_other_is_reset_checkpoint()
    {
        //Arrange
        GameObject resetCheckpoint = new GameObject();
        Collider2D collider = resetCheckpoint.AddComponent<BoxCollider2D>();
        resetCheckpoint.layer = LayerMask.NameToLayer("Reset Checkpoint");

        manager.When(x => x.Cycle()).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "OnTriggerEnter2D", new object[] { collider });

        //Assert
        ReflectionUtils.AssertMethodIsCalled(manager, "Cycle");
    }

    [Test]
    public void _1_2_Test_OnTriggerEnter2D_other_is_reposition_checkpoint()
    {
        //Arrange
        GameObject repositionCheckpointGO = new GameObject();
        Collider2D collider = repositionCheckpointGO.AddComponent<BoxCollider2D>();
        repositionCheckpointGO.layer = LayerMask.NameToLayer("Reposition Checkpoint");

        float newHeight = 2f;

        tools.When(x => x.LimitedRandomVariance(Arg.Any<float>(), Arg.Any<float>(), Arg.Any<float>(), Arg.Any<float>())).DoNotCallBase();
        tools.LimitedRandomVariance(Arg.Any<float>(), Arg.Any<float>(), Arg.Any<float>(), Arg.Any<float>()).Returns(newHeight);

        manager.When(x => x.RepositionY(Arg.Any<float>())).DoNotCallBase();

        RepositionCheckpointWrapper repositionCheckpointWrapper = Substitute.For<RepositionCheckpointWrapper>();
        repositionCheckpointWrapper.When(x => x.SetLastObstacleHeight(Arg.Any<float>())).DoNotCallBase();

        MethodInfo getRepositionCheckpointWrapperFromGameObjectMethodInfo = ReflectionUtils.GetMethod(manager, "GetRepositionCheckpointWrapperFromGameObject");
        getRepositionCheckpointWrapperFromGameObjectMethodInfo.Invoke(manager, new object[] {repositionCheckpointGO}).Returns(repositionCheckpointWrapper);
        manager.When(x => getRepositionCheckpointWrapperFromGameObjectMethodInfo.Invoke(x, new object[] {repositionCheckpointGO})).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "OnTriggerEnter2D", new object[] { collider });

        //Assert
        ReflectionUtils.AssertMethodIsCalled(manager, "RepositionY", new object[] { newHeight });
        ReflectionUtils.AssertMethodIsCalled(repositionCheckpointWrapper, "SetLastObstacleHeight", new object[] { newHeight });
    }

    [Test]
    public void _1_3_Test_OnTriggerEnter2D_other_is_player()
    {
        //Arrange
        GameObject player = new GameObject();
        Collider2D collider = player.AddComponent<BoxCollider2D>();
        player.layer = LayerMask.NameToLayer("Player");

        bool eventRaised = false;
        System.Action<GameObjectWrapper> EventRaised = new System.Action<GameObjectWrapper>((x) => { eventRaised = true; });
        EventManager.OnObstacleClear += EventRaised;

        //Act
        ReflectionUtils.Invoke(manager, "OnTriggerEnter2D", new object[] { collider });
        EventManager.OnObstacleClear -= EventRaised;

        //Assert
        Assert.AreEqual(true, eventRaised);
    }

    [Test]
    public void _2_1_Test_WakeUp_obstacle_is_asleep()
    {
        //Arrange
        obstacle.When(x => x.IsSleeping()).DoNotCallBase();
        obstacle.IsSleeping().Returns(true);

        obstacle.When(x => x.WakeUp()).DoNotCallBase();

        //Act
        ReflectionUtils.Invoke(manager, "WakeUp");

        //Assert
        ReflectionUtils.AssertMethodIsCalled(obstacle, "WakeUp");
        Assert.AreEqual(new Vector2(-Consts.GAME_X_SCROLLING_SPEED, 0), obstacle.velocity);
    }

    [Test]
    public void _2_2_Test_WakeUp_obstacle_is_awake()
    {
        //Arrange
        obstacle.When(x => x.IsSleeping()).DoNotCallBase();
        obstacle.IsSleeping().Returns(false);

        //Act
        ReflectionUtils.Invoke(manager, "WakeUp");

        //Assert
        ReflectionUtils.AssertMethodIsNotCalled(obstacle, "WakeUp");
    }
}
