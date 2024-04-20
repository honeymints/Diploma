using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField] private List<Color> bottleColors;
    [SerializeField] private SpriteRenderer bottleMask;

    [SerializeField] private AnimationCurve ScaleAndRotationCurve;
    [SerializeField] private AnimationCurve fillAmountCurve;
    [SerializeField] private AnimationCurve rotationSpeedCurve;

    [SerializeField] private float rotationTime=1f;

    [SerializeField] private float[] fillAmounts;
    [SerializeField] private float[] rotationAngles;

    [Range(0,4)]
    [SerializeField] private int numberOfColors;
    private int rotationIndex = 0;

    public Color TopColor;
    private int numberOfTopColorLayers=1;
    
    private int numberOfColorsToAdd;
    
    public BottleController bottleContrRef;
    [SerializeField] private Transform leftRotation;
    [SerializeField] private Transform rightRotation;
    
    private Transform chosenRotation;
    private float directionMultiplier=1.0f;
    
    public LineRenderer lineRenderer;
    
    public Vector3 originalPosition;
    public Vector3 startPosition;
    public Vector3 endPosition;

    public List<Color> expectedBottleColor;
    public List<Color> addedColorsToBottle;
    
    public int matchedCount;

    void Start()
    {
        if (numberOfColors < 4)
        {
            for (int i = numberOfColors; i < 4; i++)
            {
                bottleColors.AddRange(new Color[]{Color.black });
            }
            
        }
        bottleMask.material.SetFloat("_FillAmount", fillAmounts[numberOfColors]);
        
        addedColorsToBottle.AddRange(bottleColors.Where(x=>x!=Color.black));
        
        originalPosition = transform.position;
        UpdateColors();
        UpdateTopColors();
    }

    public void CheckIndexesAndMatch()
    {
        if (addedColorsToBottle.Count == expectedBottleColor.Count)
        {
            for (int i = 0; i < expectedBottleColor.Count; i++)
            {
                if ((int)addedColorsToBottle[i].r * 1000 == (int)expectedBottleColor[i].r * 1000)
                {
                    matchedCount++;
                }
                else
                {
                    matchedCount--;
                }
            }
        }
        else
        {
            matchedCount = 0;
        }
    }

    public bool CheckIfInitialMatchedWithExpectedBottle()
    {
        return matchedCount == expectedBottleColor.Count;
    }
    
    public void UpdateColors()
    {
        bottleMask.material.SetColor("_C1", bottleColors[0]);
        bottleMask.material.SetColor("_C2", bottleColors[1]);
        bottleMask.material.SetColor("_C3", bottleColors[2]);
        bottleMask.material.SetColor("_C4", bottleColors[3]);
    }

    public void StartColorTransferring()
    {
        ChoseRotationAndDirection();
        numberOfColorsToAdd = Mathf.Min(numberOfTopColorLayers, 4-bottleContrRef.numberOfColors);
        for (int i=0;i<numberOfColorsToAdd;i++)
        {
            bottleContrRef.bottleColors[bottleContrRef.numberOfColors + i] = TopColor;
            bottleContrRef.addedColorsToBottle.Add(TopColor);
            addedColorsToBottle.Remove(TopColor);
        }

        bottleContrRef.UpdateColors();
        
        CalculateRotationIndex(4-bottleContrRef.numberOfColors);

        GetComponent<SpriteRenderer>().sortingOrder += 2;
        bottleMask.sortingOrder += 2;
        
        StartCoroutine(MoveBottle());
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
            eulerAngle = Mathf.Lerp(0f, directionMultiplier*rotationAngles[rotationIndex], lerp);
            t += Time.deltaTime*rotationSpeedCurve.Evaluate(eulerAngle);

            transform.RotateAround(chosenRotation.position, Vector3.forward, lastEulerAngle-eulerAngle);
            bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

            if (fillAmounts[numberOfColors] > fillAmountCurve.Evaluate(eulerAngle))
            {
                if (lineRenderer.enabled == false)
                {
                    lineRenderer.startColor = TopColor;
                    lineRenderer.endColor = TopColor.linear;
                    lineRenderer.SetPosition(0, chosenRotation.position);
                    lineRenderer.SetPosition(1, chosenRotation.position-Vector3.up*1.45f);

                    lineRenderer.enabled = true;
                }
                
                bottleMask.material.SetFloat("_FillAmount",fillAmountCurve.Evaluate(eulerAngle));
                bottleContrRef.FillUpTheBottle(fillAmountCurve.Evaluate(lastEulerAngle)-fillAmountCurve.Evaluate(eulerAngle));
            }

            t += Time.deltaTime * rotationSpeedCurve.Evaluate(eulerAngle);
            lastEulerAngle = eulerAngle;
            
            yield return new WaitForEndOfFrame();
        }

        eulerAngle = directionMultiplier*rotationAngles[rotationIndex];

        bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));
        bottleMask.material.SetFloat("_FillAmount",fillAmountCurve.Evaluate(eulerAngle));

        numberOfColors -= numberOfColorsToAdd;
        bottleContrRef.numberOfColors += numberOfColorsToAdd;
        
        lineRenderer.enabled = false;
        StartCoroutine(RotateBottleBackToPlace());
    }
    
    IEnumerator MoveBottle()
    {
        startPosition = transform.position;

        endPosition = chosenRotation == leftRotation ? bottleContrRef.rightRotation.position : bottleContrRef.leftRotation.position;

        float t = 0;
        
        while(t<=1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }
        
        transform.position = endPosition;
        
        StartCoroutine(RotateBottle());
    }

    IEnumerator MoveBottleBack()
    {
        startPosition = transform.position;
        endPosition = originalPosition;
        
        float t = 0;
        while(t<=1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;
        
        GetComponent<SpriteRenderer>().sortingOrder -= 2;
        bottleMask.sortingOrder -= 2;
    }

    IEnumerator RotateBottleBackToPlace()
    {
        float t = 0;
        float lerp;
        float eulerAngle;

        float lastEulerAngle = directionMultiplier * rotationAngles[rotationIndex];
        while (t<rotationTime)
        {
            lerp = t / rotationTime;
            eulerAngle = Mathf.Lerp(directionMultiplier * rotationAngles[rotationIndex], 0f, lerp);
            t += Time.deltaTime;
            
            transform.RotateAround(chosenRotation.position, Vector3.forward, lastEulerAngle-eulerAngle);
            
            bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

            lastEulerAngle = eulerAngle;
            
            yield return new WaitForEndOfFrame();
        }
        
        UpdateTopColors();
        
        eulerAngle = 0;
        transform.eulerAngles = new Vector3(0, 0, eulerAngle);
        bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

        StartCoroutine(MoveBottleBack());
    }

    public void UpdateTopColors()
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

    private void CalculateRotationIndex(int numberOfEmptySpacesInSecondBottle)
    {
        rotationIndex = 3 - (numberOfColors -
                             Mathf.Min(numberOfEmptySpacesInSecondBottle, numberOfTopColorLayers));
    }

    private void FillUpTheBottle(float fillAmountToAdd)
    {
        bottleMask.material.SetFloat("_FillAmount",bottleMask.material.GetFloat("_FillAmount")+fillAmountToAdd);
    }

    private void ChoseRotationAndDirection()
    {
        if (transform.position.x>bottleContrRef.transform.position.x)
        {
            chosenRotation = leftRotation;
            directionMultiplier = -1.0f;
        }
        else
        {
            chosenRotation = rightRotation;
            directionMultiplier = 1.0f;
        }
    }
}

