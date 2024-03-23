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
    // Start is called before the first frame update
    void Start()
    {
        UpdateColors();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        while (t<rotationTime)
        {
            lerp = t / rotationTime;
            eulerAngle = Mathf.Lerp(0f, 90f, lerp);
            t += Time.deltaTime*rotationSpeedCurve.Evaluate(eulerAngle);

            transform.eulerAngles = new Vector3(0, 0, eulerAngle);
            bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));
            bottleMask.material.SetFloat("_FillAmount",fillAmountCurve.Evaluate(eulerAngle));
            
            yield return null;
        }

        eulerAngle = 90f;
        transform.eulerAngles = new Vector3(0, 0, eulerAngle);
        bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

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
            eulerAngle = Mathf.Lerp(90f, 0f, lerp);
            t += Time.deltaTime;

            transform.eulerAngles = new Vector3(0, 0, eulerAngle);
            bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));

            yield return null;
        }

        eulerAngle = 0f;
        transform.eulerAngles = new Vector3(0, 0, eulerAngle);
        bottleMask.material.SetFloat("_ScaleAndRotation", ScaleAndRotationCurve.Evaluate(eulerAngle));
    }
}
