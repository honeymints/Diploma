using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PG2Level", menuName ="PG2Level")]
public class PG2Level : ScriptableObject
{
    public int Row, Col;
    public List<Vector4> Points;
    public List<Vector2Int> Edges;
}
