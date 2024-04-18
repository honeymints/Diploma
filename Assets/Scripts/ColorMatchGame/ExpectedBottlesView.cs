using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpectedBottlesView : MonoBehaviour
{
    public ExpectedBottleData ExpectedBottleData;

    public List<SpriteRenderer> bottleMasks;
    
    void Start()
    {
        InitializeBotlle();
        SetColorForBottles();
    }

    public void SetColorForBottles()
    {
        int count = ExpectedBottleData.ExpectedBottles.Count;
        for (int i = 0; i < count; i++)
        {
            SetColorForBottle(i);
        }
    }
        
    public void SetColorForBottle(int i)
    {
        var fillAmount = -0.25f;
        for (int k = 0; k < ExpectedBottleData.ExpectedBottles[i].ExpectedBottleColors.Count; k++)
        {
            fillAmount += 0.105f;
            bottleMasks[i].material.SetColor($"_C{k+1}", ExpectedBottleData.ExpectedBottles[i].ExpectedBottleColors[k]);
            bottleMasks[i].material.SetFloat("_FillAmount", fillAmount);
        }
    }

    public void InitializeBotlle()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bottleMasks.Add(transform.GetChild(i).GetChild(0).GetComponentInChildren<SpriteRenderer>());
        }
    }

}
