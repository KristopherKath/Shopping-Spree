using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] private GameObject gameEndButtonsGroup;

    public GameObject gameEndFirstButton;

    public bool gameOver;

    private GameState State;
    Player player;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        gameOver = false;
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
        //set game over
        gameOver = true;

        //disable player input
        player.DisablePlayerGameplayInputMap();

        //show game end buttons
        gameEndButtonsGroup.SetActive(true);

        //Set current selected button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameEndFirstButton);
    }
}

public enum GameState
{ 
    GameIntro,
    GameStart,
    GameEnd
}

