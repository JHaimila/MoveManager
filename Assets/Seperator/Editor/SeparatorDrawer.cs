using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer
{
    //Get access to the window so you can draw
    public override void OnGUI(Rect position)
    {
        //Get a reference to the attribute
        SeparatorAttribute separator = attribute as SeparatorAttribute;

        //Define the line to draw
        // Xmin is the smallest x it can be. To account for window resizing
        Rect separatorRect = new Rect(position.xMin, position.yMin + separator.spacing, position.width, separator.height);

        //Draw it
        EditorGUI.DrawRect(separatorRect, Color.cyan);
    }

    public override float GetHeight()
    {
        SeparatorAttribute separator = attribute as SeparatorAttribute;
        // Calculates the height based on the separator space above, the height of the separator, then the space below the separator
        return separator.spacing + separator.spacing + separator.height;
    }
}
