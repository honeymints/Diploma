using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIView;
using UnityEngine;
using UnityEngine.UI;

namespace ColorMatchGame
{
    public class GameController : BaseController
    {
        [SerializeField] private BottleController FirstBottle;
        [SerializeField] private BottleController SecondBottle;
        [SerializeField] private List<BottleController> Bottles;
        public static GameController Instance { get; private set; }
        private float currentTime = 0;
        private float currentPoints = 0;
        private bool isAllFull=false;
        private bool HasPlayerWon = false;

        
        void Start()
        {
            Time.timeScale = 1f;
            Bottles.AddRange(FindObjectsOfType<BottleController>());
        }

        void Update()
        {
            ClickOnBottles();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartCountDown(Image timeImage, float duration, TMP_Text timeText)
        {
            StartCoroutine(GameUtils.CountDown(timeImage, duration, timeText, UpdateTime, OnTimeEnd));
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

                                FirstBottle = null;
                                SecondBottle = null;
                            }
                        }
                    }
                }
            }

            if (!isAllFull)
            {
                StartCoroutine(CheckIfAllBottlesAreMatched(3f));
            }
        }

        private void UpdateTime(float timeLeft)
        {
            currentTime = timeLeft;  
        }

        private void OnTimeEnd()
        {
            if (!HasPlayerWon)
            {
                Debug.Log("LOST!");
            }
        }
        private IEnumerator CheckIfAllBottlesAreMatched(float timeLeft)
        {
            if(Bottles.All(x => x.CheckIfInitialMatchedWithExpectedBottle()))
            {
                isAllFull = true;

                yield return new WaitForSeconds(timeLeft);

                HasPlayerWon = true;
                Win();
            }
        }

        protected void Win()
        {
            base.Win<GameController>();
        }
    }
}
