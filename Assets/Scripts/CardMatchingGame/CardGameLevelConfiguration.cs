using UnityEngine;

namespace CardMatchingGame
{
    [CreateAssetMenu(fileName = "CardLevelData", menuName = "Data/CardGame")]
    public class CardGameLevelConfiguration : ScriptableObject
    {
        public int sizeOfColumns;
        public int sizeOfRows;
        public float timeDurationForLevel;
    }
}
