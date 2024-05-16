using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private BottleController FirstBottle;
    [SerializeField] private BottleController SecondBottle;
    [SerializeField] private List<BottleController> Bottles;
    [SerializeField] private PanelView _panelView;
    private bool isAllFull=false;

    void Start()
    {
        Time.timeScale = 1f;
        Bottles.AddRange(FindObjectsOfType<BottleController>());
    }

    void Update()
    {
        ClickOnBottles();
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
            StartCoroutine(CheckIfAllBottlesAreMatched(4f));
        }
    }

    private void Win()
    {
        _panelView.ShowPanel("Nice job!");
        GetComponent<GameController>().enabled = false;
    }

    private IEnumerator CheckIfAllBottlesAreMatched(float timeLeft)
    {
        if(Bottles.All(x => x.CheckIfInitialMatchedWithExpectedBottle()))
        {
            isAllFull = true;

            yield return new WaitForSeconds(timeLeft);
            
            Win();
        }
    }
}
