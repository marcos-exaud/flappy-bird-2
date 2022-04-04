using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuManagerTests
{
    private MenuManager manager;

    [SetUp]
    public void Setup()
    {
        InitComponents();
    }
    public void InitComponents()
    {
        manager = Substitute.ForPartsOf<MenuManager>();
    }

    [Test]
    public void MenuManagerTestsSimplePasses()
    {
        
    }
}
