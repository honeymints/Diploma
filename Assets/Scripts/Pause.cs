using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private float previousTimeScale = 1f;
    
    public void TogglePause()
    {
        gameObject.SetActive(true);
        previousTimeScale = Time.timeScale;
        Time.timeScale=0;        
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale=previousTimeScale;
    }
    
}
