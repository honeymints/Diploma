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
    public TextMeshProUGUI textResult;
    public GameType GameType;
    public Dictionary<GameType, Dictionary<int, float>> HighScores=new Dictionary<GameType, Dictionary<int, float>>();
    public float moneyAmount;
    private string playfabId;

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
    
    
    #endregion

    #region Player Data
    public void UpdateScore(GameType gameType, int level, float points)
    {
        if (!HighScores.ContainsKey(gameType))
        {
            HighScores[gameType] = new Dictionary<int, float>();
        }

        HighScores[gameType][level] = points;
        SaveUserData(gameType);
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
            if (HighScores.ContainsKey(GameType.WaterColorSort) && HighScores[GameType.WaterColorSort].ContainsKey(1))
            {
                textResult.text = $"WaterColorSort Level 1 Score: {HighScores[GameType.WaterColorSort][1]}";
            }
            else
            {
                textResult.text = HighScores.Values.ToString();
            }
        }
        else
        {
            Debug.LogError("No user data available");
            textResult.text = "Didn't get the data";
        }
    }

    /*private void GetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("PlayerScores"))
        {
            string json = result.Data["PlayerScores"].Value;
            HighScores = JsonUtility.FromJson<SerializableDictionary<string, Dictionary<int, float>>>(json).ToDictionary();
            textResult.text = HighScores["WaterColorSort"].Values.ToString();
        }
        else
        {
            Debug.LogError("Key was not set");
            textResult.text = "didn't get the data";
        }
    }*/

    private void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetPlayFabId(string playfabId)
    {
        this.playfabId = playfabId;
    }
    public float GetUserHighScore(GameType gameType, int level)
    {
        if (HighScores[gameType].ContainsKey(level))
        {
            return HighScores[gameType][level];
        }

        return -1;
    }
    
    #endregion
}

