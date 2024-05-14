using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelManager Instance { get; private set; }
    public BottleColorMatchLevelConfiguration currentLevelConfig;

    private const string prefabPath="Prefabs/Bottle";
    private const string expectedBottlePrefabPath = "Prefabs/Expected Bottle";
    private const string expectedBottlesView = "Prefabs/ExpectedBottlesView";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        if (currentLevelConfig != null)
        {
            ConfigureLevel();
        }
        else
        {
            Debug.LogError("This level is not configured!");
        }
    }

    private void ConfigureLevel()
    {
        int count = 0;
        float offset = 0;
        foreach (var bottle in currentLevelConfig.InitialBottle)
        {
            try
            {
                Debug.Log($"bottle {count} was instantiated");
                GameObject initialBottle = Instantiate(Resources.Load(prefabPath)) as GameObject;
                if (initialBottle != null) initialBottle.gameObject.name = $"Bottle {count}";
                initialBottle.GetComponent<BottleController>().InitializeColors(bottle.InitialColorBottles);
                initialBottle.transform.position = new Vector3(initialBottle.transform.position.x+offset,initialBottle.transform.position.y, initialBottle.transform.position.z);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            count++;
            offset += 1f;
        }

        List<GameObject> listOfExpectedBottles = new List<GameObject>(); 
        try
        {
            GameObject expectedBottleView = Instantiate(Resources.Load(expectedBottlesView)) as GameObject;
            if (expectedBottleView != null)
            {
                expectedBottleView.gameObject.name = $"ExpectedBottleView";
                for (int i = 0; i < currentLevelConfig.ExpectedBottles.Count; i++)
                {
                    GameObject expectedBottle = Instantiate(Resources.Load(expectedBottlePrefabPath), expectedBottleView.transform) as GameObject;
                    listOfExpectedBottles.Add(expectedBottle);
                }
                expectedBottleView.GetComponent<ExpectedBottlesView>().CreateBotlles(currentLevelConfig.ExpectedBottles);
            }
            else
            {
                Debug.LogError("Expected Bottle View Prefab was not found!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
    }
}
