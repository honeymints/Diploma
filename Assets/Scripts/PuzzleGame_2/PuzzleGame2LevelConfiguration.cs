using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PG2Level", menuName ="LevelConfig/OneLineGameLevelConfig")]
public class PuzzleGame2LevelConfiguration : ScriptableObject
{
    public int Row, Col;
    public Gradient edgeColor;
    public List<Vector4> Points;
    public List<Vector2Int> Edges;
    public float timeDurationForLevel;
}
