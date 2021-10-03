using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroTimer : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] private float timeTillEnd = 5f;

    TextMeshProUGUI timeTextDisplay;
    bool enableTimer = false;

    private void Awake()
    {
        timeTextDisplay = GetComponent<TextMeshProUGUI>();
        GameManager.OnGameStateChanged += GameManagerOnOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnStateChanged;

    }

    private void Update()
    {
        if (enableTimer)
        {
            if (timeTillEnd > 0)
                timeTillEnd -= Time.deltaTime;
            else
            {
                timeTillEnd = 0;
                GameManager.Instance.UpdateGameState(GameState.GameStart); //update Game State to start state
            }

            DisplayTime(timeTillEnd); //display current time left
        }
    }

    private void GameManagerOnOnStateChanged(GameState newState)
    {
        if (newState == GameState.GameIntro)
        {
            timeTextDisplay.gameObject.SetActive(true);
            enableTimer = true;
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
        else if (timeTodisplay > 0)
        {
            timeTodisplay += 1;
        }

        float seconds = Mathf.FloorToInt(timeTodisplay % 60);

        if (timeTextDisplay)
        {
            timeTextDisplay.text = string.Format("{0:0}", seconds);
        }
    }
}
