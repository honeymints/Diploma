using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ExpectedBottleData", menuName = "Data/ExpectedBottle")]
public class ExpectedBottleData : ScriptableObject
{
    public List<ExpectedBottle> ExpectedBottles;
}
[Serializable]
public class ExpectedBottle
{
    public List<Color> ExpectedBottleColors;
}