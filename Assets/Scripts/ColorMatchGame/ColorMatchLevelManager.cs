using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGame
{
    public class ColorMatchLevelManager : MonoBehaviour
    {
        public BottleColorMatchLevelConfiguration currentLevelConfig;
        [SerializeField] private GameObject timePrefab;
        [SerializeField] private BaseController GameController;

        private const string prefabPath="Prefabs/Bottle";
        private const string expectedBottlePrefabPath = "Prefabs/Expected Bottle";
        private const string expectedBottlesView = "Prefabs/ExpectedBottlesView";

        private void Awake()
        {
            InitializeLevel();
        }

        void Start()
        {
            GameController.SetFullTime<BaseController>(currentLevelConfig.timeDurationForLevel);
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
            int count = 0;
            float offset = 0;
        
            try
            {
                foreach (var bottle in currentLevelConfig.InitialBottle)
                {
                    Debug.Log($"bottle {count} was instantiated");
                    GameObject initialBottle = Instantiate(Resources.Load(prefabPath)) as GameObject;
                    if (initialBottle != null)
                    {
                        initialBottle.gameObject.name = $"Bottle {count}";
                        initialBottle.GetComponent<BottleController>().InitializeColors(bottle.InitialColorBottles);
                        initialBottle.GetComponent<BottleController>().InitializeExpectedBottleColors(currentLevelConfig.ExpectedBottles[count].ExpectedBottleColors);
                        initialBottle.transform.position = new Vector3(currentLevelConfig.initialHorizontalPos+offset,initialBottle.transform.position.y, initialBottle.transform.position.z);
                        
                    }
                    else
                    {
                        Debug.LogError("Initial Bottle Prefab was not found!");
                    }

                    offset += currentLevelConfig.bottleDistance;
                    count++;
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }

            offset = 0f;
            List<GameObject> listOfExpectedBottles = new List<GameObject>(); 
            try
            {
                GameObject expectedBottleView = Instantiate(Resources.Load(expectedBottlesView)) as GameObject;
                if (expectedBottleView != null)
                {
                    expectedBottleView.gameObject.name = $"ExpectedBottleView";
                    for (int i = 0; i < currentLevelConfig.ExpectedBottles.Count; i++)
                    {
                        GameObject expectedBottle = Instantiate(Resources.Load(expectedBottlePrefabPath), expectedBottleView.transform) as GameObject;
                        expectedBottle.name = $"expected bottle {i}";
                        expectedBottle.transform.position=new Vector3(currentLevelConfig.expectedBottleHorizontalPos+offset,expectedBottle.transform.position.y, expectedBottle.transform.position.z);
                        listOfExpectedBottles.Add(expectedBottle);
                        offset += currentLevelConfig.expectedBottleDistance;
                    }
                    expectedBottleView.GetComponent<ExpectedBottlesView>().CreateBotlles(currentLevelConfig.ExpectedBottles);
                }
                else
                {
                    Debug.LogError("Expected Bottle View Prefab was not found!");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        
        }

    }
}
