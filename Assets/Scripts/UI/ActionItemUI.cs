using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionItemUI : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private TextMeshProUGUI _moveName;
    [SerializeField] private TextMeshProUGUI _winsText;
    private string _actionString;
    public void PopulateUI(Sprite img, string moveName, List<MoveClass> winsTo)
    {
        _img.sprite = img;
        _moveName.text = moveName;
        _actionString = moveName;
        _winsText.text = PopulateList(winsTo);
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
        _moveName.text = "Selected";
    }
    public void SetDefault()
    {
        _moveName.text = _actionString;
    }

}

