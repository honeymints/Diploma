using System;
using TMPro;
using UIView;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGame
{
    public class BaseController : MonoBehaviour
    {
        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject winPanel;

        protected float currentTime = 0;
        protected float totalTime = 0f;
        protected float currentPoints = 0;
        protected bool _hasPlayerWon = false;

        protected void Disable<T>() where T : BaseController
        {
            GetComponent<T>().enabled = false;
        }

        protected void Win<T>(float collectedPoints) where T : BaseController
        {
            Time.timeScale = 0f;
            winPanel.SetActive(true);
            GameObject.FindWithTag("EditableText").GetComponent<TMP_Text>().text = collectedPoints.ToString();
            Disable<T>();
        }
        
        protected void Lose<T>() where T : BaseController
        {
            Time.timeScale = 0f;
            losePanel.SetActive(true);
            Disable<T>();
        }

        public void StartCountDown<T>(Image timeImage, float duration, TMP_Text timeText) where T : BaseController
        {
            StartCoroutine(GameUtils.CountDown(timeImage, duration, timeText, UpdateTime<T>, OnTimeEnd<T>));
        }
        
        protected void UpdateTime<T>(float timeLeft) where T : BaseController
        {
            GetComponent<T>().currentTime = timeLeft;
        }

        
        protected void OnTimeEnd<T>() where T : BaseController
        {
            if (!_hasPlayerWon)
            {
                Lose<T>();
            }
        }

        public void SetFullTime<T>(float totalTime) where T : BaseController
        {
            this.totalTime = totalTime;
        }
    }
}
