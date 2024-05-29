using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorMatchGame
{
    [CreateAssetMenu(fileName = "BottleColorLevelConfig", menuName = "LevelConfig/FirstGameLevelConfig")]
    public class BottleColorMatchLevelConfiguration : ScriptableObject
    {
        /*public int levelNumber;*/
        public float timeDurationForLevel;
        public List<ExpectedBottle> ExpectedBottles;
        public List<InitialBottle> InitialBottle;
        public float bottleDistance;
        public float initialHorizontalPos;
        public float expectedBottleDistance;
        public float expectedBottleHorizontalPos;
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
}