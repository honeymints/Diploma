using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private GameObject StarsImg;

    private int starsCount = 0;

    void OnEnable()
    {
        
    }

    public void ShowPanel(float highScore,float collectedPoints)
    {
        highScoreText.text = highScore.ToString();
        currentScoreText.text = collectedPoints.ToString();
    }

    public void CountStars(float maxPointsForGame, float currentPointsForGame)
    {
        if (currentPointsForGame<=maxPointsForGame/3)
        {
            starsCount = 1;
        }
        else if (currentPointsForGame>maxPointsForGame/3 && currentPointsForGame<=(2*maxPointsForGame)/3)
        {
            starsCount = 2;
        }
        else
        {
            starsCount = 3;
        }
    }

    public void EnableStars()
    {
        int count=0;
        int childCount = StarsImg.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (count < starsCount)
            {
                if (!StarsImg.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    StarsImg.transform.GetChild(i).gameObject.SetActive(true);
                    count++;
                }
            }
        }
    }
}
