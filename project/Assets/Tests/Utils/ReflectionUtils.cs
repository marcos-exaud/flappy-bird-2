using System;
using System.Linq;
using System.Reflection;
using NSubstitute;

/// <summary>
/// Reflection Utilities class used on Unit Tests
/// </summary>
public class ReflectionUtils
{
    /// <summary>
    /// Invokes method based on instance, method's name and parameters
    /// </summary>
    public static object Invoke(object instance, string name, object[] args = null)
    {
        // List of the Argument Types
        Type[] argTypes = null;

        if (instance.GetType().Name != instance.GetType().BaseType.Name + "Proxy")
        {
            return instance.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, args);
        }
        // Checks for null list of arguments or any null argument inside of it
        else if (args == null || args.Any(x => x == null))
        {
            return instance.GetType().BaseType.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, args);
        }
        else
        {
            // Adding all argument types on the list in order to be used in GetMethod
            argTypes = new Type[args.Length];
            for (int i = 0; i < argTypes.Length; i++)
            {
                argTypes[i] = args[i].GetType();
            }

            return instance.GetType().BaseType.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, argTypes, null).Invoke(instance, args);
        }

    }

    /// <summary>
    /// Gets the value of a variable based on instance and variable name
    /// </summary>
    public static T GetValue<T>(object instance, string name)
    {
        FieldInfo myFieldInfo;

        if (instance.GetType().Name != instance.GetType().BaseType.Name + "Proxy")
        {
            myFieldInfo = instance.GetType().GetField(name, BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Instance | BindingFlags.Static); ;
        }
        else
        {
            myFieldInfo = instance.GetType().BaseType.GetField(name, BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Instance | BindingFlags.Static);
        }

        return (T)myFieldInfo.GetValue(instance);
    }

    /// <summary>
    /// Sets the Value of a variable based on instance and variable name
    /// </summary>
    public static void SetValue(object instance, string name, object value)
    {
        FieldInfo myFieldInfo;

        if (instance.GetType().Name != instance.GetType().BaseType.Name + "Proxy")
        {
            myFieldInfo = instance.GetType().GetField(name, BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Instance);
        }
        else
        {
            myFieldInfo = instance.GetType().BaseType.GetField(name, BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Instance);
        }
        myFieldInfo.SetValue(instance, value);
    }

    /// <summary>
    /// Gets a method based on instance and the method's name
    /// </summary>
    public static MethodInfo GetMethod(object instance, string name)
    {
        return instance.GetType().BaseType.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
    }

    /// <summary>
    /// Verifies that a Method was called based on it's "parent's" instance and Name
    /// </summary>
    public static void AssertMethodIsCalled(object instance, string name, params object[] args)
    {
        instance.Received().InvokeProtected(name, args);
    }

    public static void AssertMethodIsCalled(object instance, string name, int numberOfCalls, params object[] args)
    {
        instance.Received(numberOfCalls).InvokeProtected(name, args);
    }

    /// <summary>
    /// Verifies that a Method was not called based on it's "parent's" instance and Name
    /// </summary>
    public static void AssertMethodIsNotCalled(object instance, string name, params object[] args)
    {
        instance.DidNotReceive().InvokeProtected(name, args);
    }
}

/// <summary>
/// Class that represents all the Extension Methods used on the Reflection process
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Allows Invocation of Methods through extension
    /// </summary>
    public static object InvokeProtected(this object target, string name, params object[] args)
    {
        return ReflectionUtils.Invoke(target, name, args);
    }
}
