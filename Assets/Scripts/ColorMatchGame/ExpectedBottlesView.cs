using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpectedBottlesView : MonoBehaviour
{
    public List<SpriteRenderer> bottleMasks;

    public void SetColorForBottles(List<ExpectedBottle> expectedBottle)
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            SetColorForBottles(i, expectedBottle[i].ExpectedBottleColors);
        }
    }
        
    public void SetColorForBottles(int i, List<Color> expectedBottleColors)
    {
        var fillAmount = -0.25f;
        for (int k = 0; k < expectedBottleColors.Count; k++)
        {
            fillAmount += 0.105f;
            bottleMasks[i].material.SetColor($"_C{k + 1}", expectedBottleColors[k]);
            bottleMasks[i].material.SetFloat("_FillAmount", fillAmount);
        }
    }

    public void CreateBotlles(List<ExpectedBottle> expectedBottle)
    {
        for (int i = 0; i < expectedBottle.Count; i++)
        {
            bottleMasks.Add(transform.GetChild(i).GetChild(0).GetComponentInChildren<SpriteRenderer>());
        }
        SetColorForBottles(expectedBottle);
    }

}
