using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelListCreator : MonoBehaviour
{
    [MenuItem("Tools/Generate Scene List")]
    public static void Generate()
    {
        string currentScenePath = SceneManager.GetActiveScene().path;
        List<string> sceneNames = GetSpecificScenesFromBuildSettings(currentScenePath);

        // Create or load the SceneList ScriptableObject
        SceneList sceneList = AssetDatabase.LoadAssetAtPath<SceneList>("Assets/Scripts/Data/SceneList - Game #4.asset");
        if (sceneList == null)
        {
            // If SceneList.asset doesn't exist, create it
            sceneList = ScriptableObject.CreateInstance<SceneList>();
            AssetDatabase.CreateAsset(sceneList, "Assets/Scripts/Data/SceneList - Game #4.asset");
        }

        // Update the scene list
        sceneList.sceneNames = sceneNames.ToArray();

        // Save changes
        EditorUtility.SetDirty(sceneList);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static List<string> GetSpecificScenesFromBuildSettings(string currentScenePath)
    {
        List<string> sceneNames = new List<string>();

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string currentFolder = currentScenePath.Substring(0, currentScenePath.LastIndexOf('/')); // Get the folder path of the current scene

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.enabled)
            {
                string scenePath = scene.path;
                int lastSlashIndex = scenePath.LastIndexOf('/');
                string sceneNameWithExtension = scenePath.Substring(lastSlashIndex + 1);
                string sceneName = sceneNameWithExtension.Substring(0, sceneNameWithExtension.Length - 6); // Remove ".unity" extension

                if (scenePath.StartsWith(currentFolder) && sceneName != SceneManager.GetActiveScene().name) 
                {
                    sceneNames.Add(sceneName);
                }
            }
        }

        return sceneNames;
    }
}
