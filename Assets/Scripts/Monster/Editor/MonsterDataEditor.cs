using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterClass))]
public class MonsterDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MonsterClass data = (MonsterClass)target;
        ProgressBar(data.GetElo(), "Difficulty");
        EditorGUILayout.LabelField(data.moveName.ToUpper(), EditorStyles.boldLabel);
        //Add Before
        base.OnInspectorGUI();
    }

    void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");

        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space(10);
    }
}
