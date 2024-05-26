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
    
    public static void CountPoints(float totalTime, float timeLeft, ref float currentPoints)
    {
        var t = totalTime / 3;
        Debug.Log(t);
        if (t>timeLeft)
        {
            currentPoints += .1f*currentPoints;
        }
        else if (timeLeft>=t && timeLeft<totalTime/2f)
        {
            currentPoints += .2f*currentPoints;
        }
        else if(timeLeft>=totalTime/2f && timeLeft<totalTime/1.5f)
        {
            currentPoints += .3f*currentPoints;
        }
        else
        {
            currentPoints += .4f*currentPoints;
        } 
    }

    public static string GameTypeDetector(GameType gameType)
    {
        string type="";
        switch(gameType)
        {
            case GameType.BlocksGame:
                type = "BlockGames";
                break;
            case GameType.CardMatch:
                type = "CardMatching";
                break;
            case GameType.OneLineGame:
                type = "OneLine";
                break;
            case GameType.WaterColorSort:
                type = "WaterColorSort";
                break;
        }

        return type;
    }

}
