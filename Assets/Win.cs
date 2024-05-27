using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    public void ShowPanel(float highScore,float collectedPoints)
    {
        highScoreText.text = highScore.ToString();
        currentScoreText.text = collectedPoints.ToString();
    }
}
