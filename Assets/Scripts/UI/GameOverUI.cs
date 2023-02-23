using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;
    public void SetUI(string text)
    {
        textUI.text = text;
        gameObject.SetActive(true);
    }
    public void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
