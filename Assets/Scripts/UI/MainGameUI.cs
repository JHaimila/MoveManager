using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MainGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _movesParent;
    [SerializeField] private GameObject _movesUI;
    [SerializeField] private Image _enemyImage;
    [SerializeField] private Image _playerImage;
    [SerializeField] private Button _select;
    [SerializeField] private GameHandler _gameHandler;
    [SerializeField] private Sprite _defaultImg;
    private SelectedAction _selectedMove;

    private void Start() {
        ResetUI();
    }
    public void SetPlayerImage(Sprite sprite)
    {
        _playerImage.sprite = sprite;
    }
    public void SetEnemyImage(Sprite sprite)
    {
        _enemyImage.sprite = sprite;
    }
    public void UpdateActionsList(List<MoveClass> moves)
    {
        if(_movesParent.transform.childCount > 0)
        {
            for(int i = 0; i<_movesParent.transform.childCount; i++)
            {
                Destroy(_movesParent.transform.GetChild(i).gameObject);
            }
        }
        Vector3 offset = Vector3.zero;
        foreach(var move in moves)
        {
            GameObject listItem = Instantiate(_movesUI, _movesParent.transform.position - offset, Quaternion.identity, _movesParent.transform);
            listItem.GetComponent<ActionItemUI>().PopulateUI(move.img, move.moveName, move.beats);
            listItem.GetComponent<Button>().onClick.AddListener(delegate {SetSelect(move, listItem.GetComponent<ActionItemUI>());});
            offset += new Vector3(0, _movesUI.GetComponent<RectTransform>().rect.height, 0);
        }
    }
    public void SetSelect(MoveClass move, ActionItemUI ui)
    {
        if(_selectedMove != null)
        {
            _selectedMove.ui.SetDefault();
        }
        _selectedMove = new SelectedAction();
        _selectedMove.move = move;
        _selectedMove.ui = ui;
        ui.SetActive();
    }
    public void ConfirmSelection()
    {
        if(_selectedMove != null)
        {
            _gameHandler.SetPlayerAction(_selectedMove.move);
            _gameHandler.PlayRound();
        }
    }
    public void ResetUI()
    {
        _selectedMove = null;
        SetPlayerImage(_defaultImg);
        SetEnemyImage(_defaultImg);
    }
}
public class SelectedAction
{
    public MoveClass move;
    public ActionItemUI ui;
}
