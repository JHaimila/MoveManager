using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MoveClass))]
public class MoveAbilityDrawer : PropertyDrawer
{
    private SerializedProperty _name;
    private SerializedProperty _damge;
    private SerializedProperty _element;

    // How to draw to the inspector window
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }

    //request more virtical space
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
