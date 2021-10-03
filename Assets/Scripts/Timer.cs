using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeTillEnd = 90f;
    [SerializeField] private TextMeshProUGUI timeTextDisplay;


    void Update()
    {
        if (timeTillEnd > 0)
            timeTillEnd -= Time.deltaTime;
        else
        {
            timeTillEnd = 0;
        }

        DisplayTime(timeTillEnd);
    }

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
