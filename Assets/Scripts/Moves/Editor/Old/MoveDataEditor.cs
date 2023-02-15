using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveClass))]
public class MoveDataEditor : Editor
{
    private SerializedProperty moveName;
    private SerializedProperty beats;
    // [Separator(2, 12)]
    private SerializedProperty img;
    private SerializedProperty damage;
    [Range(0, 100)]
    private SerializedProperty wear;
    private SerializedProperty doesCombat;
    private SerializedProperty moveAbilities;

    private void OnEnable() {
        moveName = serializedObject.FindProperty("moveName");
        beats = serializedObject.FindProperty("beats");
        img = serializedObject.FindProperty("img");
        damage = serializedObject.FindProperty("damage");
        wear = serializedObject.FindProperty("wear");
        doesCombat = serializedObject.FindProperty("doesCombat");
        moveAbilities = serializedObject.FindProperty("moveAbilities");
    }

    public override void OnInspectorGUI()
    {
        //Updates the serialized object if we make a change
        serializedObject.UpdateIfRequiredOrScript();

        // Less protected and old way of doing this. SerializedProperty and SerializedObject are better and newer
        // MoveClass data = (MoveClass)target;

        // ProgressBar(data.GetElo(), "Difficulty");
        EditorGUILayout.LabelField(moveName.stringValue.ToUpper(), EditorStyles.boldLabel);
        
        // Draws the base inspector
        // base.OnInspectorGUI();

        EditorGUILayout.LabelField("General Stats", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(moveName, new GUIContent("Name"));
        if(moveName.stringValue == "")
        {
            EditorGUILayout.HelpBox("No name set for this move, is that intentional?", MessageType.Warning);
        }
        EditorGUILayout.PropertyField(doesCombat, new GUIContent("Does Combat"));
        if(doesCombat.boolValue)
        {
            EditorGUILayout.LabelField("Combat Info", EditorStyles.boldLabel);
            // ProgressBar(damage.floatValue * (NormalizeValue(0, damage.floatValue, damage.floatValue * (wear.floatValue/100))), "Move Elo");
            EditorGUI.indentLevel++;
            // EditorGUILayout.BeginHorizontal();
            // EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.PropertyField(damage, new GUIContent("Damage"));
            EditorGUILayout.PropertyField(wear, new GUIContent("Wear"));
            EditorGUILayout.PropertyField(moveAbilities, new GUIContent("Abilities"));
            // Button
            if(GUILayout.Button("Random Stats"))
            {
                RandomizeStats();
            }
            // EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.PropertyField(beats, new GUIContent("Wins Against"));
        EditorGUILayout.PropertyField(img, new GUIContent("Move Icon"));
        
        

        

        // Helpers
        // if(beats.)
        // {
        //     EditorGUILayout.HelpBox("You have set this move to beat itself, is that intentional?", MessageType.Error);
        // }
        

        // Applies the save if any changes were made
        serializedObject.ApplyModifiedProperties();
    }

    void RandomizeStats()
    {
        damage.floatValue = Random.Range(1, 50);
        wear.floatValue = Random.Range(1, 50);
    }
    void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");

        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space(10);
    }
    public float NormalizeValue(float min, float max, float num)
    {
        float normalizeTop = (num-min);
        float normalizeBottom = (max-min);
        return (normalizeTop / normalizeBottom);
    }
}
