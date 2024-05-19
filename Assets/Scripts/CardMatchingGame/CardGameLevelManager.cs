using System;
using System.Collections;
using System.Collections.Generic;
using CardMatchingGame;
using ColorMatchGame;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardGameLevelManager : MonoBehaviour
{
        public CardGameLevelConfiguration currentLevelConfig;
        [SerializeField] private GameObject timePrefab;
        [SerializeField] private BaseController GameController;
        [SerializeField] private GameObject _panel;

        [SerializeField] private GameObject _cardPrefab;
        private const string prefabPath="Prefabs/СardButton";
        void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            if (currentLevelConfig != null)
            {
                ConfigureLevel();
                TMP_Text timeText = timePrefab.GetComponentInChildren<TMP_Text>();
                Image timeImg = timePrefab.GetComponent<Image>();
                GameController.StartCountDown<BaseController>(timeImg, currentLevelConfig.timeDurationForLevel, timeText);
            }
            else
            {
                Debug.LogError("This level configuration is not set!");
            }
        }

        private void ConfigureLevel()
        {
            
            try
            {
                List<Button> cards = new List<Button>();
                for (int i = 0; i < currentLevelConfig.sizeOfColumns * currentLevelConfig.sizeOfRows; i++)
                {
                    var cardPrefab=Resources.Load(prefabPath) as GameObject;
                    
                    var card = Instantiate(_cardPrefab, _cardPrefab.transform.position, Quaternion.identity);
                    card.transform.SetParent(_panel.transform);
                    card.name = $"{_cardPrefab.name} #{i + 1}";
                    card.transform.localScale = Vector3.one;
                    cards.Add(card.GetComponent<Button>());
                    
                }
                
                GameController.GetComponent<CardGameController>().CreateCards(cards);

                _panel.GetComponent<GridLayoutGroup>().constraintCount = currentLevelConfig.sizeOfColumns;

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        
        }
}
