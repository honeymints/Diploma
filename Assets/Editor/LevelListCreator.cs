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
        List<string> sceneNames = new List<string>();
        List<int> buildIndexes = new List<int>();
        
        GetSpecificScenesFromBuildSettings(currentScenePath, sceneNames, buildIndexes);
        
        // Create or load the SceneList ScriptableObject
        SceneList sceneList = AssetDatabase.LoadAssetAtPath<SceneList>("Assets/Data/SceneList - Game #2.asset");
        if (sceneList == null)
        {
            // If SceneList.asset doesn't exist, create it
            sceneList = ScriptableObject.CreateInstance<SceneList>();
            AssetDatabase.CreateAsset(sceneList, "Assets/Data/SceneList - Game #2.asset");
        }

        // Update the scene list
        sceneList.sceneNames = sceneNames.ToArray();
        sceneList.buildIndexes = buildIndexes.ToArray();

        // Save changes
        EditorUtility.SetDirty(sceneList);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void GetSpecificScenesFromBuildSettings(string currentScenePath, List<string> sceneNames, List<int> buildIndexes)
    {

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string currentFolder = currentScenePath.Substring(0, currentScenePath.LastIndexOf('/')); // the folder path of the current scene

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.enabled)
            {
                string scenePath = scene.path;
                int lastSlashIndex = scenePath.LastIndexOf('/');
                string sceneNameWithExtension = scenePath.Substring(lastSlashIndex + 1);
                // removes ".unity" extension
                string sceneName = sceneNameWithExtension.Substring(0, sceneNameWithExtension.Length - 6); 

                if (scenePath.StartsWith(currentFolder) && sceneName != SceneManager.GetActiveScene().name) 
                {
                    sceneNames.Add(sceneName);
                    buildIndexes.Add(SceneUtility.GetBuildIndexByScenePath(scenePath));
                }
            }
        }

    }
}
