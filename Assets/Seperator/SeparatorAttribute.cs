using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, 
AllowMultiple = true)]
public class SeparatorAttribute : PropertyAttribute
{
    public readonly float height;
    public readonly float spacing;
    public SeparatorAttribute(float _height = 1, float _spacing = 10)
    {
        height = _height;
        spacing = _spacing;
    }
}
