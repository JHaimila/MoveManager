using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionItemUI : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI moveName;
    [SerializeField] private TextMeshProUGUI winsText;
    private string _actionString;
    public void PopulateUI(Sprite img, string moveName, List<MoveClass> winsTo)
    {
        this.img.sprite = img;
        this.moveName.text = moveName;
        _actionString = moveName;
        winsText.text = PopulateList(winsTo);
    }
    
    private string PopulateList(List<MoveClass> moves)
    {
        string finalText = "";
        foreach(var move in moves)
        {
            finalText += ", "+move.moveName;
        }
        finalText = finalText.Remove(0,1);
        finalText = "Beats: "+finalText;
        return finalText;
    }

    public void SetActive()
    {
        moveName.text = "Selected";
    }
    public void SetDefault()
    {
        moveName.text = _actionString;
    }

}

