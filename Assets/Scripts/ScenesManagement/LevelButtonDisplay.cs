using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private Image[] fillStars;
    [SerializeField] private Image[] emptyStars;

    public void Init(float score)
    {
        scoreTxt.text = score.ToString();
    }

    public void DisplayStars(int starsCount)
    {
        int count = 0;
        for (int i = 0; i < emptyStars.Length; i++)
        {
            if (count < starsCount)
            {
                if (!fillStars[i].gameObject.activeInHierarchy)
                {
                    fillStars[i].gameObject.SetActive(true);
                    count++;
                }
            }
        }
    }
}
