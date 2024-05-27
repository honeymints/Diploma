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
    
    public static bool IsEqualTo(Color me, Color other)
    {
        Debug.Log($"here is reds: {(int)me.r*1000}, {(int)other.r * 1000}");
        Debug.Log($"here is greens: {(int)(me.g * 1000)}, {(int)(me.g * 1000)}");
        Debug.Log($"here is blues: {(int)(me.b * 1000)}, {(int)(other.b * 1000)}");
        return Mathf.Abs(me.r-other.r) <0.1f && Mathf.Abs(me.b-other.b) <0.1f &&
               Mathf.Abs(me.g-other.g) <0.1f;
    }

}
