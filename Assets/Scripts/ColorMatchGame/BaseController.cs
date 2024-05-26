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
        [SerializeField] protected PlayerData PlayerData;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject winPanel;
        private UserAccountController UserController;
        
        protected float currentTime = 0;
        protected float totalTime = 0f;
        protected float currentPoints = 0;
        protected bool _hasPlayerWon = false;


        protected GameType gameType;
        protected float HighScore;
        protected int currentLevelNumber;
        
        protected void Start()
        {
            UserController = FindObjectOfType<UserAccountController>();
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
        
        protected void Disable<T>() where T : BaseController
        {
            GetComponent<T>().enabled = false;
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
        
        protected void OnGameCompleted<T>() where T : BaseController
        {
            string type=GameUtils.GameTypeDetector(GetComponent<T>().gameType);
            int level = GetComponent<T>().currentLevelNumber;
            float highScore = 100f;
            UserController.UpdateScore(type, level, highScore);
        }

        public void SetFullTime<T>(float totalTime) where T : BaseController
        {
            this.totalTime = totalTime;
        }
    }
}
