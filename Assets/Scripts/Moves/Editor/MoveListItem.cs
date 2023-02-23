using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

public class MoveListItem : VisualElement
{
    private Button moveButton;
    private VisualElement issueIndicator;
    
    public MoveListItem(string name, List<string> validationIssues)
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MoveManager.path + "MoveListItem.uxml");
        Add(original.Instantiate());

        moveButton = this.Q<Button>("moveButton");
        issueIndicator = this.Q<VisualElement>("invalidIcon");
        
        moveButton.text = name;
        issueIndicator.visible = validationIssues.Count > 0;
        if (issueIndicator.visible)
        {
            issueIndicator.tooltip = "No win condition for: ";
            foreach (var issue in validationIssues)
            {
                issueIndicator.tooltip += issue+", ";
            }
        }
    }

    public Button GetButton()
    {
        return moveButton;
    }
}
