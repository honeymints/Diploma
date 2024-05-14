using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEditor.Playables;
using UnityEngine;

public class Configuration : MonoBehaviour
{
    private Grid grid;

    [SerializeField] private WalkablePositions _walkablePositions;
    // Start is called before the first frame update
    void Start()
    {
         grid = new Grid(5,8, 1f, transform.position, _walkablePositions);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 65);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
