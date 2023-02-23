using System;
using UnityEngine;
using TMPro;
public class GameStatusText : MonoBehaviour
{
    private TextMeshProUGUI gameStatusText;

    private void Start()
    {
        gameStatusText = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string newText)
    {
        gameStatusText.text = newText;
    }
}
