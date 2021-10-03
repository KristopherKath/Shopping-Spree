using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsCalculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsTextDisplay;
    [SerializeField] private TextMeshProUGUI multiPointsTextDisplay;
    [SerializeField] private TextMeshProUGUI totalPointsTextDisplay;

    ItemStack itemStack;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnStateChanged; //subscribe to game manager state change
        itemStack = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemStack>();    
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnStateChanged; //unsubscribe to game manager state change
    }

    private void GameManagerOnOnStateChanged(GameState state)
    {
        if (state == GameState.GameEnd)
        {
            CalculatePoints();
            pointsTextDisplay.gameObject.SetActive(true);
            multiPointsTextDisplay.gameObject.SetActive(true);
            totalPointsTextDisplay.gameObject.SetActive(true);

        }
        else
        {
            pointsTextDisplay.gameObject.SetActive(false);
            multiPointsTextDisplay.gameObject.SetActive(false);
            totalPointsTextDisplay.gameObject.SetActive(false);
        }
    }

    void CalculatePoints()
    {
        int totalPoints = 0;
        
        int points = itemStack.GetTotalValue() * 100;
        
        float multiplier = itemStack.GetTotalWeight() * 1;

        int multiPoints = (int)(((float)points) * (1 + multiplier));

        totalPoints = points + multiPoints;

        Debug.Log("points points: " + points);
        Debug.Log("weight points: " + multiPoints);
        Debug.Log("total points: " + totalPoints);

        pointsTextDisplay.text = string.Format("Points: {0:0}", points);
        multiPointsTextDisplay.text = string.Format("Multiplier Points: {0:0}", multiPoints);
        totalPointsTextDisplay.text = string.Format("Total Points: {0:0}", totalPoints);

    }
}
