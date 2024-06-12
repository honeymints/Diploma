using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColorMatchGame
{
    public class ColorMatchGameController : BaseController
    {
        [SerializeField] private BottleController FirstBottle;
        [SerializeField] private BottleController SecondBottle;
        [SerializeField] private List<BottleController> Bottles;

        private bool isAllFull=false;
        private int countOfMatchedBottles;
        private bool coroutineRunning = false;
        
        private float bottleUp = 0.3f; // select bottle
        private float bottleDown = -0.3f; // deselect bottle

        void Start()
        {
            currentTime = 0f;
            currentPoints = 100f;
            maxScoreForGame = 140f;
            Time.timeScale = 1f;

            currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            gameType = GameType.WaterColorSort;
            HighScore = GetHighScore<ColorMatchGameController>();
            
            Bottles.AddRange(FindObjectsOfType<BottleController>());
        }

        void Update()
        {
            ClickOnBottles();
        }

        public void IncrementCountOfMatchedBottles()
        {
            var allMatched = Bottles.All(x => x.matchedCount == x.expectedBottleColors.Count);
            CheckIfAllBottlesAreMatched(allMatched);
        }
        private void ClickOnBottles()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<BottleController>() != null)
                    {
                        if (FirstBottle == null)
                        {
                            FirstBottle = hit.collider.GetComponent<BottleController>();
                            
                            if(FirstBottle.numberOfColors != 0)
                            {
                                FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                    FirstBottle.transform.position.y + bottleUp,
                                    FirstBottle.transform.position.z);
                            }
                        }
                        else
                        {
                            if (FirstBottle == hit.collider.GetComponent<BottleController>())
                            {
                                if(FirstBottle.numberOfColors != 0)
                                {
                                    FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                                        FirstBottle.transform.position.y + bottleDown,
                                        FirstBottle.transform.position.z);
                                }
                                
                                FirstBottle = null;
                            }
                            else
                            {
                                SecondBottle = hit.collider.GetComponent<BottleController>();
                                FirstBottle.bottleContrRef = SecondBottle;

                                FirstBottle.UpdateTopColors();
                                SecondBottle.UpdateTopColors();

                                FirstBottle.StartColorTransferring();

                                SecondBottle.CheckIndexesAndMatch();
                                FirstBottle.CheckIndexesAndMatch();
                                IncrementCountOfMatchedBottles();

                                FirstBottle = null;
                                SecondBottle = null;
                            }
                        }
                    }
                }
                else {      
                    if(FirstBottle.numberOfColors != 0)
                    {
                        FirstBottle.transform.position = new Vector3(FirstBottle.transform.position.x,
                            FirstBottle.transform.position.y + bottleDown,
                            FirstBottle.transform.position.z);
                    }
                    FirstBottle = null;
                    SecondBottle = null;
                }
            }

        }

        private void CheckIfAllBottlesAreMatched(bool allMatched)
        {
            if (allMatched)
            {
                Debug.Log("matched");
                isAllFull = true;
                Invoke("HandleWin", 3f); 
            }
        
        }

        private void HandleWin()
        {
            if (isAllFull)
            {
                _hasPlayerWon = true;
                GameUtils.CountPoints(totalTime, currentTime, ref currentPoints);
                OnGameCompleted<ColorMatchGameController>();
                Win<ColorMatchGameController>(this.currentPoints, this.HighScore, this.maxScoreForGame);
            }
        }
        
    }
}

