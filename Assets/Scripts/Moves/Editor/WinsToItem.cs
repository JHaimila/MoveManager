using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class WinsToItem : VisualElement
{
    Button removeWinsTo;
    Label winsToLabel;

    public WinsToItem(string beatsName)
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MoveManager.path + "WinsToItem.uxml");
        this.Add(original.Instantiate());

        removeWinsTo = this.Q<Button>();
        winsToLabel = this.Q<Label>();

        winsToLabel.text = beatsName;
    }

    public Button GetButton(){return removeWinsTo;}
    public Label GetLabel(){return winsToLabel;}
}
