using System;
using TMPro;
using UIView;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

    public class BaseController : MonoBehaviour
    {
        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject winPanel;

        protected float currentTime = 0;
        protected float totalTime = 0f;
        protected float currentPoints = 0;
        protected bool _hasPlayerWon = false;
        
        protected float maxScoreForGame;
        protected GameType gameType;
        protected float HighScore;
        protected int currentLevelIndex;

        protected void Win<T>(float collectedPoints, float highScore, float maxPoints) where T : BaseController
        {
            Time.timeScale = 0f;
            winPanel.SetActive(true);
            winPanel.GetComponent<Win>().ShowPanel(highScore, collectedPoints);
            winPanel.GetComponent<Win>().CountStars(maxPoints,collectedPoints);
            winPanel.GetComponent<Win>().EnableStars();
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
            SetHighScore<T>();
            int level = GetComponent<T>().currentLevelIndex;
            GameType gameType = GetComponent<T>().gameType;
            /*Debug.Log("high score is: " + HighScore);
            UserAccountController.UserController.UpdateScore(gameType, level, GetComponent<T>().HighScore);*/
        }

        protected float GetHighScore<T>() where T : BaseController
        {
            int level = GetComponent<T>().currentLevelIndex;
            GameType gameType = GetComponent<T>().gameType;
            float highScore = UserAccountController.UserController.GetUserHighScore(gameType, level);
            if (highScore != 0)
            {
                GetComponent<T>().HighScore = highScore;
            }
            return highScore;
        }
        
        private void SetHighScore<T>() where T : BaseController
        {
            if (GetComponent<T>().currentPoints > GetComponent<T>().HighScore)
            {
                GetComponent<T>().HighScore = GetComponent<T>().currentPoints;
            }
        }
        public void SetFullTime<T>(float totalTime) where T : BaseController
        {
            this.totalTime = totalTime;
        }
        
        
    }

