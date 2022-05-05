using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public abstract class OneInstance : MonoBehaviour//, OneInstanceInterface
{
    public static Dictionary<string, object> AllInstances = new Dictionary<string, object>();

    //public abstract OneInstance instance { get; set; }
    void Awake()
    {
    }

    public OneInstance()
    {
    }


    public static void AddInstance<T>(string name, T value)
    {
        AllInstances.Add(name, value);
    }

    /// <summary> 
    /// Replaces instance of type with new instance,
    /// functionally reseting it.
    /// </summary>
    abstract public void ResetInstance();


    public static void resetAllInstances()
    {
        foreach (var instance in AllInstances.Values)
        {
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty("instance");
            if (propertyInfo.GetValue(instance) != null)
            {
                propertyInfo.SetValue(null, null);
            }
        }

        AllInstances.Clear();
    }
}

interface OneInstanceInterface
{
    OneInstance instance { get; set; }
}
