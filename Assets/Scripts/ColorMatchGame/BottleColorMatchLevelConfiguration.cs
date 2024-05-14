using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BottleColorLevelConfig", menuName = "LevelConfig/FirstGameLevelConfig")]
public class BottleColorMatchLevelConfiguration : ScriptableObject
{
    public string levelName;
    public List<ExpectedBottle> ExpectedBottles;
    public List<InitialBottle> InitialBottle;
    
}
[Serializable]
public class InitialBottle
{
    public List<Color> InitialColorBottles;
}

[Serializable]
public class ExpectedBottle
{
    public List<Color> ExpectedBottleColors;
}
