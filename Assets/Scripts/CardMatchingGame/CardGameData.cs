using UnityEngine;

namespace CardMatchingGame
{
    [CreateAssetMenu(fileName = "CardLevelData", menuName = "Data/CardGame")]
    public class CardGameData : ScriptableObject
    {
        public int sizeOfColumns;
        public int sizeOfRows;
    }
}
