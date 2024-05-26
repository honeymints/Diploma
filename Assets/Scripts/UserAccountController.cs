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
    public string playfabId;

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

    #region PlayerStats
    
    public GameType GameType;
    public Dictionary<string, Dictionary<int, float>> HighScores=new Dictionary<string, Dictionary<int, float>>();
    public float moneyAmount;
    
    public void SetStatistics()
    {
        PlayFabClientAPI.UpdatePlayerStatistics( new UpdatePlayerStatisticsRequest {
                // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate() {StatisticName = "MoneyAmount", Value = 100},
                }
            },
            result => { Debug.Log("User statistics updated"); },
            error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch (eachStat.StatisticName)
            {
                case "MoneyAmount":
                    moneyAmount = eachStat.Value;
                    break;
                
            }
        }
        
    }
    
    #endregion

    #region Player Data
    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(new SerializableDictionary<string, Dictionary<int, float>>(HighScores));

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { "PlayerScores", json } }
        };

        PlayFabClientAPI.UpdateUserData(request, result => {
            Debug.Log("Successfully updated user data");
        }, error => {
            Debug.Log("Got error setting user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    public void UpdateScore(string gameType, int level, float points)
    {
        if (!HighScores.ContainsKey(gameType))
        {
            HighScores[gameType] = new Dictionary<int, float>();
        }

        HighScores[gameType][level] = points;

        // After updating the score, save the player data
        SaveUserData();
    }
    
    public void GetUserData(string playerId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = playerId // Include the player's ID
            }, 
            result => {
                Debug.Log("Got user data:");
                if (result.Data != null && result.Data.ContainsKey("PlayerScores"))
                {
                    string json = result.Data["PlayerScores"].Value;
                    HighScores = JsonUtility.FromJson<SerializableDictionary<string, Dictionary<int, float>>>(json).ToDictionary();
                }
            }, 
            error => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
    }

    public void GetPlayFabId(string playfabId)
    {
        this.playfabId = playfabId;
    }
    public Dictionary<string, Dictionary<int,float>> GetUserData()
    {
        GetUserData(playfabId);
        return HighScores;
    }
    #endregion
}

