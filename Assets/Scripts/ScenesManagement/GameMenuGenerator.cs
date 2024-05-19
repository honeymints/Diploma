using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Directory = System.IO.Directory;

public class GameMenuGenerator : MonoBehaviour
{
    [SerializeField] private GameObject gridLayout; 
    
    private const string sceneDirectoryPath = "Assets/Scenes";
    private const string bottleGameScenePath = "Assets/Scenes/Game #1";
    private const string cardGameScenePath = "Assets/Scenes/Game #2";
    private const string levelButtonPrefabPath="Prefabs/LevelButton";

    private List<string> scenePaths = new List<string>();
    private string currentScene;
    
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        string filePath = GetFilePath(sceneDirectoryPath, $"{currentScene}.unity");
        filePath = filePath.Replace("\\", "/");
        
        if (!string.IsNullOrEmpty(filePath))
        {
            Debug.Log($"File found at path: {filePath}");
            scenePaths = GetFilesFromDirectory(Path.GetDirectoryName(filePath));
        }
        else
        {
            Debug.Log("File not found.");
        }
        
        GenerateButtons();
    }

    private string GetFilePath(string directoryPath, string fileName)
    {
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"This directory path doesn't exist: {directoryPath}");
            return null;
        }
        
        var file = Directory.GetFiles(directoryPath, fileName, SearchOption.AllDirectories).FirstOrDefault();

        return file;
    }
    
    private void GenerateButtons()
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
    }
    
    private List<string> GetFilesFromDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"this directory directoryPath doesn't exist: {directoryPath}");
            return new List<string>();
        }
        else
        {
            var directoryPaths = Directory.GetFiles(directoryPath, "*.unity", SearchOption.AllDirectories)
                .Where(x=>!x.Contains($"{currentScene}.unity"));
            return new List<string>(directoryPaths);
        }
    }
}
