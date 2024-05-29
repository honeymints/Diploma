using UnityEngine;
using TMPro;
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
            button.GetComponentInChildren<TMP_Text>().text = (count + 1).ToString();
            button.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
            count++;
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
