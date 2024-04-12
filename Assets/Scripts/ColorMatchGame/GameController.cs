using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private BottleController FirstBottle;
    [SerializeField] private BottleController SecondBottle;
    
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
            Debug.Log(hit);

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

                            if (SecondBottle.CheckBottleFilled(FirstBottle.TopColor))
                            {
                               FirstBottle.StartColorTransferring();
                            }
                            
                            FirstBottle = null;
                            SecondBottle = null;
                            
                        }
                    }
                }
            }
        }
    }
}
