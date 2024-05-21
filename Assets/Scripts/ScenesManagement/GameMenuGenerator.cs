using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuGenerator : MonoBehaviour
{
    [SerializeField] private GameObject gridLayout; 

    private const string levelButtonPrefabPath="Prefabs/LevelButton";

    public SceneList sceneList;

    void Start()
    {
        if (sceneList == null || sceneList.sceneNames.Length == 0)
        {
            Debug.LogError("No scene list found.");
            return;
        }

        GenerateButtons(sceneList.sceneNames);
    }

    private void GenerateButtons(string[] sceneNames)
    {
        int count = 0;
        foreach (var sceneName in sceneNames)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>(levelButtonPrefabPath);
            if (buttonPrefab == null)
            {
                Debug.LogError("Button prefab could not be loaded from Resources.");
                continue;
            }
            GameObject button = Instantiate(buttonPrefab, gridLayout.transform);
            button.GetComponentInChildren<TMPro.TMP_Text>().text = (count + 1).ToString();
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => LoadScene(sceneName));
            count++;
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /*void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        string filePath = GetFilePath(sceneDirectoryPath, $"{currentScene}.unity");
        filePath = filePath.Replace("\\", "/");#1#
        
        /*if (!string.IsNullOrEmpty(filePath))
        {
            Debug.Log($"File found at path: {filePath}");
            scenePaths = GetFilesFromDirectory(Path.GetDirectoryName(filePath));
        }
        else
        {
            Debug.Log("File not found.");
        }
        
        
        GenerateButtons();
    }*/
    

    /*private string GetFilePath(string directoryPath, string fileName)
    {
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"This directory path doesn't exist: {directoryPath}");
            return null;
        }
        
        var file = Directory.GetFiles(directoryPath, fileName, SearchOption.AllDirectories).FirstOrDefault();

        return file;
    }*/
    
    /*private void GenerateButtons()
    {
        int count = 0;
        foreach (var scenePath in scenePaths)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>(levelButtonPrefabPath);
            GameObject button = Instantiate(buttonPrefab, GameObject.FindWithTag("Layout").transform);
            button.GetComponentInChildren<TMP_Text>().text=(count+1).ToString();
            
            button.GetComponent<Button>().onClick.AddListener(() => LoadScene(scenePath));
            count++;
        }
    }

    private void LoadScene(string scenePath)
    {
        string sceneName = Path.GetFileNameWithoutExtension(scenePath);
        SceneManager.LoadScene(sceneName);
    }*/
    
    /*private List<string> GetFilesFromDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"this directory directoryPath doesn't exist: {directoryPath}");
            return new List<string>();
        }
        else
        {
            var directoryPaths = Directory.GetFiles(directoryPath, "*.unity", SearchOption.AllDirectories)
                .Where(x=>!x.Contains($"{currentScene}.unity")).OrderBy(x => ExtractLevelNumber(x)).ToList();
            return new List<string>(directoryPaths);
        }
    }*/
    /*private int ExtractLevelNumber(string filePath)
    {
        var match = Regex.Match(filePath, @"Level (\d+)\.unity");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int levelNumber))
        {
            return levelNumber;
        }
        return int.MaxValue; 
    }*/

}
