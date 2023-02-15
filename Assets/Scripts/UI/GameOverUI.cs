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
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void DisableUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
