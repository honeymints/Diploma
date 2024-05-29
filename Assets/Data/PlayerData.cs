using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PLayer")]
public class PlayerData : ScriptableObject
{
    public GameType GameType;
    public float HighScore;
    public int LevelNumber;
}
public enum GameType
{
    WaterColorSort,
    CardMatch,
    BlocksGame,
    OneLineGame,
}