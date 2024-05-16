using CodeMonkey.Utils;
using UnityEngine;

namespace FoodMatcherGame
{
    public class Grid
    {
        private int height;
        private int width;
        private float cellSize;
        private int[,] gridArray;
        private TextMesh[,] debugTextArray;
        private Vector3 originPosition;
        private WalkablePositions walkablePositions;
        public Grid(int width, int height, float cellSize, Vector3 originPosition, WalkablePositions walkablePositions)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            this.walkablePositions = walkablePositions;
        
            gridArray = new int[width, height];
            debugTextArray = new TextMesh[width, height];
        
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    debugTextArray[i,j]=UtilsClass.CreateWorldText(gridArray[i, j].ToString(), null, GetWorldPosition(i,j)+new Vector3(cellSize,cellSize)*0.5f, 10, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(i,j),GetWorldPosition(i,j+1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i,j),GetWorldPosition(i+1,j), Color.white, 100f);
                }
            }
        
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width,height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width, height), Color.white, 100f);
        }

        private Vector3 GetWorldPosition(int x,int y)
        {
            return new Vector3(x, y) * cellSize+originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
        }

        public void SetValue(int x, int y, int value)
        {
            if ((x < width && y < height) && (x >= 0 && y >= 0))
            {
                int oldValue = gridArray[x, y];
                gridArray[x, y] = value;
                debugTextArray[x, y].text = gridArray[x, y].ToString();
            
                if (oldValue == 0 && value != 0)
                {
                    // Position becomes walkable
                    walkablePositions.AddPosition(new Vector2Int(x, y));
                }
                else if (oldValue != 0 && value == 0)
                {
                    // Position becomes unwalkable
                    walkablePositions.RemovePosition(new Vector2Int(x, y));
                }
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x, y;
            GetXY(worldPosition, out x,out y);
            SetValue(x,y, value);
        }
    
        public int GetValue(int x, int y)
        {
            if ((x < width && y < height) && (x >= 0 && y >= 0))
            {
                return gridArray[x, y];
            }

            return 0;
        }

        public int GetValue(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
        }
    }
}
