using System;
using UnityEngine;
using UnityEngine.UIElements;

public static class QueryExtensions
{
    public static T MQ<T>(this VisualElement e, string name = null, string className = null) where T : VisualElement
    {
        return e.MandatoryQ<T>(name, className);
    }

    public static VisualElement MQ(this VisualElement e, string name, string className = null)
    {
        return e.MandatoryQ<VisualElement>(name, className);
    }

    public static T MandatoryQ<T>(this VisualElement e, string name = null, string className = null) where T : VisualElement
    {
        T val = e.Q<T>(name, className);
        if (val == null)
        {
            throw new Exception("Element not found: " + name);
        }

        return val;
    }
}

public static class VisualElementExtensions
{
    public static void SetFlex(this VisualElement e, bool value)
    {
        if (value)
        {
            e.style.display = DisplayStyle.Flex;
        }
        else
        {
            e.style.display = DisplayStyle.None;
        }
    }
}