using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField] private Color[] bottleColors;
    [SerializeField] private SpriteRenderer bottleMask;

    [SerializeField] private AnimationCurve ScaleAndRotationCurve;
    [SerializeField] private AnimationCurve fillAmountCurve;
    [SerializeField] private AnimationCurve rotationSpeedCurve;

    [SerializeField] private float rotationTime=1f;

    [SerializeField] private float[] fillAmounts;
    [SerializeField] private float[] rotationAngles;

    [Range(0,4)]
    [SerializeField] private int numberOfColors = 4;
    private int rotationIndex = 0;

    [SerializeField] private Color TopColor;
    private int numberOfTopColorLayers=1;
    
    private int numberOfColorsToAdd;
    [SerializeField] private BottleController bottleContrRef;
    [SerializeField] private bool thisBottle=false;
    
    void Start()
    {
        bottleMask.material.SetFloat("_FillAmount", fillAmounts[numberOfColors]);
        UpdateColors();
        UpdateTopColors();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && thisBottle)
        {
            UpdateTopColors();
            
            if (bottleContrRef.CheckBottleFilled(TopColor))
            {
                numberOfColorsToAdd = Mathf.Min(numberOfTopColorLayers, 4-bottleContrRef.numberOfColors);
                for (int i=0;i<numberOfColorsToAdd;i++)
                {
                    bottleContrRef.bottleColors[bottleContrRef.numberOfColors + i] = TopColor;
                }
                bottleContrRef.UpdateColors();
            }
            CalculateRotationIndex(4-bottleContrRef.numberOfColors);
            StartCoroutine(RotateBottle());
        }
    }

    public void UpdateColors()
    {
        bottleMask.material.SetColor("_C1", bottleColors[0]);
        bottleMask.material.SetColor("_C2", bottleColors[1]);
        bottleMask.material.SetColor("_C3", bottleColors[2]);
        bottleMask.material.SetColor("_C4", bottleColors[3]);
    }

    IEnumerator RotateBottle()
    {
        float t = 0;
        float lerp;
        float eulerAngle;
        
        float lastEulerAngle=0;
        
        while (t<rotationTime)
        {
            lerp = t / rotationTime;
            eulerAngle = Mathf.Lerp(0f, rotationAngles[rotationIndex], lerp);
            t += Time.deltaTime*rotationSpeedCurve.Evaluate(eulerAngle);

            transform.eulerAngles = new Vector3(0, 0, eulerAngle);
            bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

            if (fillAmounts[numberOfColors] > fillAmountCurve.Evaluate(eulerAngle))
            {
                bottleMask.material.SetFloat("_FillAmount",fillAmountCurve.Evaluate(eulerAngle));
                
                bottleContrRef.FillUpTheBottle(fillAmountCurve.Evaluate(lastEulerAngle)-fillAmountCurve.Evaluate(eulerAngle));
            }

            t += Time.deltaTime * rotationSpeedCurve.Evaluate(eulerAngle);
            lastEulerAngle = eulerAngle;
            
            yield return new WaitForEndOfFrame();
        }

        eulerAngle = rotationAngles[rotationIndex];
        transform.eulerAngles = new Vector3(0, 0, eulerAngle);
        bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));
        bottleMask.material.SetFloat("_FillAmount",fillAmountCurve.Evaluate(eulerAngle));

        numberOfColors -= numberOfColorsToAdd;
        bottleContrRef.numberOfColors += numberOfColorsToAdd;
        
        StartCoroutine(RotateBottleBackToPlace());
    }

    IEnumerator RotateBottleBackToPlace()
    {
        float t = 0;
        float lerp;
        float eulerAngle;
        while (t<rotationTime)
        {
            lerp = t / rotationTime;
            eulerAngle = Mathf.Lerp(rotationAngles[rotationIndex], 0f, lerp);
            t += Time.deltaTime;

            transform.eulerAngles = new Vector3(0, 0, eulerAngle);
            bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

            yield return new WaitForEndOfFrame();
        }
        UpdateTopColors();
        
        eulerAngle = 0;
        transform.eulerAngles = new Vector3(0, 0, eulerAngle);
        bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));
    }

    void UpdateTopColors()
    {
        if (numberOfColors != 0)
        {
            numberOfTopColorLayers = 1;
            TopColor = bottleColors[numberOfColors-1];
            
            if (numberOfColors == 4)
            {
                if (bottleColors[3].Equals(bottleColors[2]))
                {
                    numberOfTopColorLayers = 2;
                    if (bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberOfTopColorLayers = 3;
                        if (bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberOfTopColorLayers = 4;
                        }
                    }
                }
            }

            else if (numberOfColors == 3)
            {
                if (bottleColors[2].Equals(bottleColors[1]))
                {
                    numberOfTopColorLayers = 2;
                    if (bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberOfTopColorLayers = 3;
                    }      
                }
            }

            else if (numberOfColors == 2)
            {
                if (bottleColors[1].Equals(bottleColors[0]))
                {
                    numberOfTopColorLayers = 2;
                } 
            }

            rotationIndex = 3 - (numberOfColors - numberOfTopColorLayers);
        }
    }

    private bool CheckBottleFilled(Color color)
    {
        if (numberOfColors != 0)
        {
            if (numberOfColors == 4)
            {
                return false;
            }
            else{
                if(TopColor.Equals(color))
                {
                    return true;
                }
                else{
                    return false;
                }
            }
        }
        else
        {
            return true;
        }
    }

    private void CalculateRotationIndex(int numberOfEmptySpacesInSecondBottle)
    {
        rotationIndex = 3 - (numberOfColors -
                             Mathf.Min(numberOfEmptySpacesInSecondBottle, numberOfTopColorLayers));
    }

    private void FillUpTheBottle(float fillAmountToAdd)
    {
        bottleMask.material.SetFloat("_FillAmount",bottleMask.material.GetFloat("_FillAmount")+fillAmountToAdd);
    }
}
