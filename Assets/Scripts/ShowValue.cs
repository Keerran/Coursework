using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property)]
public class ShowValue : Attribute
{
    /// <summary>
    ///  This gets all the properties from a class that contain this attribute.
    /// </summary>
    /// <typeparam name="T">The class from which to search.</typeparam>
    /// <returns>An array of the properties, sorted by the order given, if any.</returns>
    internal static PropertyInfo[] getValues<T>()
    {
        Type t = typeof(T);

        return getValues(t);
    }
    
    internal static PropertyInfo[] getValues(Type t)
    {
        
        // Gets all the properties from the given type.
        PropertyInfo[] properties = t.GetProperties().Where(
            // Selects the properties which contain the ShowValue attribute.
            prop => prop.IsDefined(typeof(ShowValue), false)
        ).ToArray(); // This changes the IEnumerable<PropertyInfo> to a PropertyInfo[].
        
        // Orders the properties by the given lambda function.
        properties = properties.OrderBy(prop =>
            // Orders it by the order given in the attribute instance.
            ((ShowValue) prop.GetCustomAttributes(typeof(ShowValue), false).First()).order
        ).ToArray();
        
        return properties;
    }

    public int order = 0;
}