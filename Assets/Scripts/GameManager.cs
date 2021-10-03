using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] private GameObject gameEndButtonsGroup;

    private GameState State;
    Player player;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        UpdateGameState(GameState.GameIntro); //start game state to intro state
    }

    public void UpdateGameState(GameState newState)
    {
        Debug.Log("Game State Change: " + newState);
        State = newState;

        switch (newState)
        {
            case GameState.GameIntro:
                GameIntro();
                break;
            case GameState.GameStart:
                GameStart();
                break;
            case GameState.GameEnd:
                GameEnd();
                break;
            default:
                break;
        }

        //any subscribed members will have event invoked when changed
        OnGameStateChanged?.Invoke(newState);
    }
    void GameIntro()
    {
        gameEndButtonsGroup.SetActive(false);
    }

    void GameStart()
    {
        player.EnablePlayerGameplayInputMap();
    }

    void GameEnd()
    {
        player.DisablePlayerGameplayInputMap();
        gameEndButtonsGroup.SetActive(true);
    }
}

public enum GameState
{ 
    GameIntro,
    GameStart,
    GameEnd
}

