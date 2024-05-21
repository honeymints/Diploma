using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGame
{
    public class BottleGameController : BaseController
    {
        [SerializeField] private BottleController FirstBottle;
        [SerializeField] private BottleController SecondBottle;
        [SerializeField] private List<BottleController> Bottles;

        private bool isAllFull=false;
        private int countOfMatchedBottles;
        private bool coroutineRunning = false;
        void Start()
        {
            currentTime = 0f;
            currentPoints = 100f;
            Time.timeScale = 1f;
            Bottles.AddRange(FindObjectsOfType<BottleController>());
        }

        void Update()
        {
            ClickOnBottles();
            
        }

        public void IncrementCountOfMatchedBottles()
        {
            var allMatched = Bottles.All(x => x.matchedCount == x.expectedBottleColors.Count);
            Debug.Log($"All matched:{allMatched}");
            /*Debug.Log("matched count " + Bottles[0].matchedCount);*/
            Debug.Log("expected bottle count" + Bottles[0].expectedBottleColors.Count);
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
                        }
                        else
                        {
                            if (FirstBottle == hit.collider.GetComponent<BottleController>())
                            {
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
            }
            
        }

        private void CheckIfAllBottlesAreMatched(bool allMatched)
        {
            /*var allMatched = true;
            foreach (var bottle in Bottles)
            {
                /*if (!bottle.CheckIfInitialMatchedWithExpectedBottle())
                {
                    Debug.Log("not matched");
                    allMatched = false;
                    break;
                }#1#
            }*/

            if (allMatched)
            {
                Debug.Log("matched");
                isAllFull = true;
                Invoke("HandleWin", 3f); // Call HandleWin method after 3 seconds
            }
        
        }

        private void HandleWin()
        {
            if (isAllFull)
            {
                _hasPlayerWon = true;
                GameUtils.CountPoints(totalTime, currentTime, ref currentPoints);
                Win<BottleGameController>(currentPoints);
            }
        }
        
    }
}

