using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuGenerator : MonoBehaviour
{
    [SerializeField] private GameObject gridLayout; 

    private const string levelButtonPrefabPath="Prefabs/LevelButton";

    public SceneList sceneList;
    public List<float> highScores = new List<float>();
    public GameType GameType;

    void Start()
    {
        if (sceneList == null || sceneList.sceneNames.Length == 0)
        {
            Debug.LogError("No scene list found.");
            return;
        }
        highScores = UserAccountController.UserController.GetDataForGameType(GameType, sceneList.buildIndexes);
        Debug.Log($"HighScores count: {highScores.Count}, SceneNames count: {sceneList.sceneNames.Length}");

        if (highScores.Count != sceneList.sceneNames.Length)
        {
            Debug.LogWarning("High scores count does not match scene names count. Padding highScores with default values.");
            while (highScores.Count < sceneList.sceneNames.Length)
            {
                highScores.Add(0);
            }
        }
        
        /*highScores.AddRange(UserAccountController.UserController.GetScoresForGameType(GameType, sceneList.buildIndexes));*/
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        int count = 0;
        
        foreach (var sceneName in sceneList.sceneNames)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>(levelButtonPrefabPath);
            if (buttonPrefab == null)
            {
                Debug.LogError("Button prefab could not be loaded from Resources.");
                continue;
            }

            if (count >= sceneList.buildIndexes.Length)
            {
                Debug.LogError("Index out of range for sceneList.buildIndexes.");
                continue;
            }

            if (count >= highScores.Count)
            {
                Debug.LogError("Index out of range for highScores.");
                continue;
            }
            
            int starsCount =
                UserAccountController.UserController.GetUserStarsCount(GameType, sceneList.buildIndexes[count]);
            GameObject button = Instantiate(buttonPrefab, gridLayout.transform);
            button.GetComponentInChildren<TMP_Text>().text = (count + 1).ToString();
            button.GetComponent<LevelButtonDisplay>().Init(highScores[count]);
            button.GetComponent<LevelButtonDisplay>().DisplayStars(starsCount);
            button.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
            count++;
        }
        /*for (int i=0; i<sceneList.sceneNames.Count();i++)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>(levelButtonPrefabPath);
            if (buttonPrefab == null)
            {
                Debug.LogError("Button prefab could not be loaded from Resources.");
                continue;
            }

            int starsCount = UserAccountController.UserController.GetUserStarsCount(GameType, sceneList.buildIndexes[i]);
            GameObject button = Instantiate(buttonPrefab, gridLayout.transform);
            button.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
            button.GetComponent<LevelButtonDisplay>().Init(highScores[i]);
            button.GetComponent<LevelButtonDisplay>().DisplayStars(starsCount);
            count++;
        }

        foreach (var sceneName in sceneList.sceneNames)
        {
            button.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
        }
        */
        
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
