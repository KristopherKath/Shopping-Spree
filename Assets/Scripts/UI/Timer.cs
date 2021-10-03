using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeTillEnd = 90f;

    TextMeshProUGUI timeTextDisplay;
    bool startCount = false;

    private void Awake()
    {
        timeTextDisplay = GetComponent<TextMeshProUGUI>();
        GameManager.OnGameStateChanged += GameManagerOnOnStateChanged; //subscribe to game manager state change

    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnStateChanged;
    }

    void Update()
    {
        //if game state start began then activate this
        if (startCount)
        {
            if (timeTillEnd > 0)
                timeTillEnd -= Time.deltaTime;
            else
            {
                timeTillEnd = 0;
                GameManager.Instance.UpdateGameState(GameState.GameEnd); //update Game State to end state
            }

            DisplayTime(timeTillEnd); //display current time left
        }
    }

    private void GameManagerOnOnStateChanged(GameState newState)
    {
        if (newState == GameState.GameStart)
        {
            startCount = true;
            timeTextDisplay.gameObject.SetActive(true);
        }
        else
        {
            timeTextDisplay.gameObject.SetActive(false);
        }
    }

    //displays time to text display
    void DisplayTime(float timeTodisplay)
    {
        if (timeTodisplay < 0)
        {
            timeTodisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeTodisplay / 60);
        float seconds = Mathf.FloorToInt(timeTodisplay % 60);
        float milliseconds = timeTodisplay % 1 * 1000;

        if (timeTextDisplay)
        {
            timeTextDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}
