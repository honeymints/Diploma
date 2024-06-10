using System;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserAccountController : MonoBehaviour
{
    public static UserAccountController UserController { get; private set; }
    public Dictionary<GameType, Dictionary<int, float>> HighScores=new Dictionary<GameType, Dictionary<int, float>>();
    private string PlayfabId;

    void Awake()
    {
        if (UserController == null)
        {
            UserController = this;
        }
        else
        {
            if (UserController != this)
            {
                Destroy(this.gameObject);
            }    
        }
        DontDestroyOnLoad(this.gameObject);
    }

    #region Player Data
    public void UpdateScore(GameType gameType, int level, float points)
    {
        if (!HighScores.ContainsKey(gameType))
        {
            HighScores[gameType] = new Dictionary<int, float>();
        }
        if (!HighScores[gameType].ContainsKey(level) || HighScores[gameType][level] < points)
        {
            HighScores[gameType][level] = points;
            SaveUserData(gameType);
        }
    }

    public void SaveUserData(GameType gameType)
    {
        if (HighScores.ContainsKey(gameType))
        {
            string json = JsonUtility.ToJson(new SerializableDictionary<int, float>(HighScores[gameType]));

            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> { { gameType.ToString(), json } }
            };

            PlayFabClientAPI.UpdateUserData(request, SetUserDataSuccess, OnErrorLeaderboard);
        }
    }

    private void SetUserDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log(result.DataVersion);
    }
    
    public void GetUserData(string playerId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playerId,
            Keys = null,
        }, GetUserDataSuccess, OnErrorLeaderboard);
    }

    private void GetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data != null)
        {
            foreach (var item in result.Data)
            {
                GameType gameType;
                if (Enum.TryParse(item.Key, out gameType))
                {
                    HighScores[gameType] = JsonUtility.FromJson<SerializableDictionary<int, float>>(item.Value.Value).ToDictionary();
                }
            }
        }
        else
        {
            Debug.LogError("No user data available");
        }
    }
    
    private void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetPlayFabId(string playfabId)
    {
        this.PlayfabId = playfabId;
    }
    public float GetUserHighScore(GameType gameType, int level)
    {

        if (HighScores.ContainsKey(gameType))
        {
            if (HighScores[gameType].ContainsKey(level))
            {
                return HighScores[gameType][level];
            }
        }
        
        return 0;
    }
    
    public List<float> GetScoresForGameType(GameType gameType)
    {
        List<float> scores = new List<float>();

        if (HighScores.ContainsKey(gameType))
        {
            foreach (var levelScore in HighScores[gameType])
            {
                scores.Add(levelScore.Value);
            }
        }

        return scores;
    }
    
    #endregion
}

