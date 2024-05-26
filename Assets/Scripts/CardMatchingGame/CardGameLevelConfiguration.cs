using UnityEngine;

namespace CardMatchingGame
{
    [CreateAssetMenu(fileName = "CardGameLevelConfig", menuName = "LevelConfig/CardGame")]
    public class CardGameLevelConfiguration : ScriptableObject
    {
        public int levelNumber;
        public int sizeOfColumns;
        public int sizeOfRows;
        public float timeDurationForLevel;
    }
}
