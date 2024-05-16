using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private int activeSceneIndex;

    private void Start()
    {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(activeSceneIndex+1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(activeSceneIndex-1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(activeSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
