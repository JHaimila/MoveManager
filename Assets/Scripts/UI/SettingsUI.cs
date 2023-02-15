using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SettingsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winsText;
    [SerializeField] private TextMeshProUGUI _lossesText;
    [SerializeField] private TextMeshProUGUI _drawsText;
    [SerializeField] private GameObject _parent;
    private bool _active;

    [SerializeField] private PlayerStats _playerStats;

    public void SetUI()
    {
        _parent.SetActive(true);
        _winsText.text = _playerStats.wins.ToString();
        _lossesText.text = _playerStats.losses.ToString();
        _drawsText.text = _playerStats.draws.ToString();
    }

    public void ExitUI()
    {
        _parent.SetActive(false);
    }
}
