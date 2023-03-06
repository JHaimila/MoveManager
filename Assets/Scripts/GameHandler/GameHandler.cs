using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameHandler : MonoBehaviour
{
    private MoveClass playerMove;
    private MoveClass enemyMove; 
    private List<MoveClass> moves;
    [SerializeField] private MainGameUI mainGameUI;
    [SerializeField] private GameStatusText gameStatusText;
    private int countDownTimer;

    private void Start() {
        moves = new List<MoveClass>();
        Object[] objects = Resources.LoadAll("Moves", typeof(MoveClass));
        foreach(var obj in objects)
        {
            moves.Add((MoveClass)obj);
        }
        mainGameUI.UpdateActionsList(moves);
    }

    public void SetPlayerAction(MoveClass move)
    {
        playerMove = move;
        mainGameUI.SetPlayerImage(move.img);
    }
    public void SetEnemyMove()
    {
        enemyMove = moves[Random.Range(0, moves.Count)];
        mainGameUI.SetEnemyImage(enemyMove.img);
    }
    private void CompareMoves()
    {
        bool winnerFound = false;
        if(enemyMove == playerMove)
        {
            StartCoroutine(WaitToShowResults("Draw! How boring!"));
            winnerFound = true;
        }
        if (!winnerFound && playerMove.beats.Contains(enemyMove))
        {
            StartCoroutine(WaitToShowResults("You won, congrats!"));
            winnerFound = true;
        }
        if (!winnerFound && enemyMove.beats.Contains(playerMove))
        {
            StartCoroutine(WaitToShowResults("You lost, you suck!"));
            winnerFound = true;
        }
        if(!winnerFound)
        {
            Debug.LogError("Unhanlded match between "+enemyMove.moveName+" and "+playerMove.moveName+" no winner can be decided");
            Reset();
        }
        
    }
    
    public void PlayRound()
    {
        SetEnemyMove();
        CompareMoves();
    }
    public void Reset()
    {
        mainGameUI.ResetUI();
        gameStatusText.SetText("Select your move");
        mainGameUI.UpdateActionsList(moves);
    }
    IEnumerator WaitToShowResults(string text)
    {
        gameStatusText.SetText(text);
        yield return new WaitForSeconds(3);
        Reset();
    }
}
