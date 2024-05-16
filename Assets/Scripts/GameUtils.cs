using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUtils
{
    public static IEnumerator CountDown(Image timeImage, float duration, TMP_Text timeText, Action<float> onTimeUpdate, Action onTimeEnd)
    {
        float currentTime = duration;
        while (currentTime>=0)
        {
            timeImage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
            timeText.text = currentTime.ToString();
            onTimeUpdate?.Invoke(currentTime);
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        onTimeEnd?.Invoke();
    }
    
    public static void CountPoints(float timeLeft, ref float currentPoints)
    {
        if (timeLeft > 0 && timeLeft <= 10f)
        {
            currentPoints += 10;
        }
        else if (timeLeft>10f && timeLeft<=20f)
        {
            currentPoints += 20;
        }
        else if(timeLeft>20f && timeLeft<=40f)
        {
            currentPoints += 30;
        }
        else
        {
            currentPoints += 40;
        } 
    }

    public static void CountPoints(float totalPoints)
    {
        
    }
    
}
