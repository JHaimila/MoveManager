using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private MoveClass _playerMove;
    private MoveClass _enemyMove; 
    private List<MoveClass> _moves;
    [SerializeField] private MainGameUI _mainGameUI;
    [SerializeField] private GameOverUI _gameOverUI;
    [SerializeField] private PlayerStats _playerStats;
    private int countDownTimer;

    private void Start() {
        _moves = new List<MoveClass>();
        Object[] objects = Resources.LoadAll("Moves", typeof(MoveClass));
        foreach(var obj in objects)
        {
            _moves.Add((MoveClass)obj);
        }
        _mainGameUI.UpdateActionsList(_moves);
    }

    public void SetPlayerAction(MoveClass move)
    {
        _playerMove = move;
        _mainGameUI.SetPlayerImage(move.img);
    }
    public void SetEnemyMove()
    {
        _enemyMove = _moves[Random.Range(0, _moves.Count)];
        _mainGameUI.SetEnemyImage(_enemyMove.img);
    }
    private void CompareMoves()
    {
        bool winnerFound = false;
        if(_enemyMove == _playerMove)
        {
            StartCoroutine(WaitToShowResults("Draw! How boring!"));
            _playerStats.draws++;
            winnerFound = true;
        }
        if(!winnerFound)
        {
            foreach(var action in _playerMove.beats)
            {
                if(action == _enemyMove)
                {
                    StartCoroutine(WaitToShowResults("You won, congrats!"));
                    _playerStats.wins++;
                    winnerFound = true;
                    break;
                }
            }
        }
        if(!winnerFound)
        {
            foreach(var action in _enemyMove.beats)
            {
                if(action == _playerMove)
                {
                    StartCoroutine(WaitToShowResults("You lost, you suck!"));
                    _playerStats.losses++;
                    winnerFound = true;
                    break;
                }
            }
        }
        if(!winnerFound)
        {
            Debug.LogError("Unhanlded match between "+_enemyMove.moveName+" and "+_playerMove.moveName+" no winner can be decided");
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
        _mainGameUI.ResetUI();
        _gameOverUI.DisableUI();
        _mainGameUI.UpdateActionsList(_moves);
    }
    IEnumerator WaitToShowResults(string text)
    {
        yield return new WaitForSeconds(1);
        _gameOverUI.SetUI(text);
    }
}
