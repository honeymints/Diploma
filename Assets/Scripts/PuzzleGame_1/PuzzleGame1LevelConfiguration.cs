using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PG1Level", menuName = "LevelConfig/BlocksameLevelConfig")]
public class PuzzleGame1LevelConfiguration : ScriptableObject
{
    public int Rows;
    public int Columns;
    public List<int> Data;
    public int BlockRows;
    public int BlockColumns;
    public List<BlockPiece> Blocks;
    public float timeDurationForLevel;
}

[Serializable]
public struct BlockPiece
{
    public int Id;
    public Vector2Int StartPos;
    public Vector2Int CenterPos;
    public List<Vector2Int> BlockPositions;
}