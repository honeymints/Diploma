using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/FoodMatchGameData", fileName = "WalkablePositionsData")]
public class WalkablePositions : ScriptableObject
{
    public List<Vector2Int> positions = new List<Vector2Int>();

    public void AddPosition(Vector2Int position)
    {
        if (!positions.Contains(position))
        {
            positions.Add(position);
        }
    }

    public void RemovePosition(Vector2Int position)
    {
        positions.Remove(position);
    }
}
