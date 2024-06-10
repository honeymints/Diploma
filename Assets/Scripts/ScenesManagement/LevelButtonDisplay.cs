using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButtonDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;

    public void Init(float score)
    {
        scoreTxt.text = score.ToString();
    }
}
