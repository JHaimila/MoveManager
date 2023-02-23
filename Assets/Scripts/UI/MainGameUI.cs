using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class MainGameUI : MonoBehaviour
{
    [SerializeField] private GameObject movesParent;
    [SerializeField] private GameObject movesUI;
    [SerializeField] private Image enemyImage;
    [SerializeField] private Image playerImage;
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private Sprite defaultImg;
    private SelectedAction selectedMove;

    private void Start() {
        ResetUI();
    }
    public void SetPlayerImage(Sprite sprite)
    {
        playerImage.sprite = sprite;
    }
    public void SetEnemyImage(Sprite sprite)
    {
        enemyImage.sprite = sprite;
    }
    public void UpdateActionsList(List<MoveClass> moves)
    {
        if(movesParent.transform.childCount > 0)
        {
            for(int i = 0; i<movesParent.transform.childCount; i++)
            {
                Destroy(movesParent.transform.GetChild(i).gameObject);
            }
        }
        Vector3 offset = Vector3.zero;
        foreach(var move in moves)
        {
            GameObject listItem = Instantiate(movesUI, movesParent.transform.position - offset, Quaternion.identity, movesParent.transform);
            listItem.GetComponent<ActionItemUI>().PopulateUI(move.img, move.moveName, move.beats);
            listItem.GetComponent<Button>().onClick.AddListener(delegate {SetSelect(move, listItem.GetComponent<ActionItemUI>());});
            offset += new Vector3(0, movesUI.GetComponent<RectTransform>().rect.height, 0);
        }
    }
    public void SetSelect(MoveClass move, ActionItemUI ui)
    {
        if(selectedMove != null)
        {
            selectedMove.ui.SetDefault();
        }
        selectedMove = new SelectedAction();
        selectedMove.move = move;
        selectedMove.ui = ui;
        ui.SetActive();
    }
    public void ConfirmSelection()
    {
        if(selectedMove != null)
        {
            gameHandler.SetPlayerAction(selectedMove.move);
            gameHandler.PlayRound();
        }
    }
    public void ResetUI()
    {
        selectedMove = null;
        SetPlayerImage(defaultImg);
        SetEnemyImage(defaultImg);
    }
}
public class SelectedAction
{
    public MoveClass move;
    public ActionItemUI ui;
}
