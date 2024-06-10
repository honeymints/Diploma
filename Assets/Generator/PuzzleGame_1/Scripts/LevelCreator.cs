#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelCreator : MonoBehaviour
{
    public static LevelCreator Instance;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private int _spawnRows;
    [SerializeField] private int _spawnColumns;
    [SerializeField] private Transform _spawnBGPrefab;
    [FormerlySerializedAs("_level")] [SerializeField] private PuzzleGame1LevelConfiguration puzzleGame1LevelConfiguration;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Transform _centerPrefab;
    [SerializeField] private float _blockSpawnSize = 0.5f;
    [SerializeField] private List<Sprite> _blockSprites;
    [SerializeField] private SpawnedBlock _blockPrefab;

    private bool isNewLevel;
    private Cell[,] gridCells;
    private int currentCellFillValue;
    private Dictionary<int, Vector2Int> startCenters;
    private List<Transform> centerObjects;
    private Dictionary<int, SpawnedBlock> spawnedBlocks;
    private Vector3 startPos;

    private void Awake()
    {
        Instance = this;
        SpawnBlock();
        SpawnGrid();
    }

    private void SpawnBlock()
    {
        isNewLevel = !(_rows == puzzleGame1LevelConfiguration.Rows && _columns == puzzleGame1LevelConfiguration.Columns);
        if (isNewLevel) 
        {
            puzzleGame1LevelConfiguration.Rows = _rows;
            puzzleGame1LevelConfiguration.Columns = _columns;
            puzzleGame1LevelConfiguration.BlockRows = _spawnRows;
            puzzleGame1LevelConfiguration.BlockColumns = _spawnColumns;
            puzzleGame1LevelConfiguration.Blocks = new List<BlockPiece>();
            puzzleGame1LevelConfiguration.Data = new List<int>();
            for(int i=0; i<_rows; i++) 
            {
                for(int j=0; j<_columns; j++) 
                {
                    puzzleGame1LevelConfiguration.Data.Add(-1);
                }
            }
        }

        gridCells = new Cell[_rows, _columns];
        for(int i=0; i<_rows;i++)
        {
            for (int j=0; j<_columns;j++)
            {
                gridCells[i, j] = Instantiate(_cellPrefab);
                gridCells[i, j].Init(puzzleGame1LevelConfiguration.Data[i*_columns + j]);
                gridCells[i, j].transform.position = new Vector3(j + 0.5f, i + 0.5f, 0);
            }
        }

        currentCellFillValue = -1;
    }

    private void SpawnGrid()
    {
        startPos = Vector2.zero;
        startPos.x = 0.25f + (puzzleGame1LevelConfiguration.Columns - puzzleGame1LevelConfiguration.BlockColumns * _blockSpawnSize) * 0.5f;
        startPos.y = -puzzleGame1LevelConfiguration.BlockRows * +_blockSpawnSize - 1f + 0.25f;

        for(int i=0; i<_spawnRows; i++)
        {
            for(int j = 0; j<_spawnColumns; j++)
            {
                Vector3 spawnPos = startPos + new Vector3(j, i, 0) * _blockSpawnSize;
                Transform spawnCell = Instantiate(_spawnBGPrefab);
                spawnCell.position = spawnPos;
            }
        }

        float maxColumns = Mathf.Max(puzzleGame1LevelConfiguration.Columns, puzzleGame1LevelConfiguration.BlockColumns * _blockSpawnSize);
        float maxRows = puzzleGame1LevelConfiguration.Rows + 2f + puzzleGame1LevelConfiguration.BlockRows * _blockSpawnSize;
        Camera.main.orthographicSize = Mathf.Max(maxColumns, maxRows) * 0.65f;
        Vector3 camPos = Camera.main.transform.position;
        camPos.x = puzzleGame1LevelConfiguration.Columns * 0.5f;
        camPos.y = (puzzleGame1LevelConfiguration.Rows + 0.5f + startPos.y) * 0.5f;
        Camera.main.transform.position = camPos;

        //set StartCenters
        startCenters = new Dictionary<int, Vector2Int>();
        centerObjects = new List<Transform>();
        spawnedBlocks = new Dictionary<int, SpawnedBlock> ();

        List<Sprite> sprites = _blockSprites;

        for(int i = 1; i < sprites.Count; i++)
        {
            spawnedBlocks[i - 1] = null;
            startCenters[i - 1] = Vector2Int.zero;
            centerObjects.Add(Instantiate(_centerPrefab));
            centerObjects[i - 1].GetChild(0).GetComponent<SpriteRenderer>().sprite = 
                sprites[i];
            centerObjects[i-1].gameObject.SetActive(false);
        }

        for(int i = 1;i < puzzleGame1LevelConfiguration.Blocks.Count; i++)
        {
            int tempId = puzzleGame1LevelConfiguration.Blocks[i].Id;
            Vector2Int pos = puzzleGame1LevelConfiguration.Blocks[i].CenterPos;
            centerObjects[tempId].gameObject.SetActive(true);
            centerObjects[tempId].transform.position =
                new Vector3(pos.y + 0.5f, pos.x + 0.5f, 0f);
            spawnedBlocks[tempId] = Instantiate(_blockPrefab);
            spawnedBlocks[tempId].Init(puzzleGame1LevelConfiguration.Blocks[i], startPos);
        }
    }
    public void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            //set grid pos
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int mousePosGrid = new Vector2Int(
                Mathf.FloorToInt(mousePos.y),
                Mathf.FloorToInt(mousePos.x)
                );
            if(!IsValidPosition(mousePosGrid)) return;
            gridCells[mousePosGrid.x, mousePosGrid.y].Init(currentCellFillValue);
            puzzleGame1LevelConfiguration.Data[mousePosGrid.x * _columns + mousePosGrid.y] = currentCellFillValue;
            EditorUtility.SetDirty(puzzleGame1LevelConfiguration);
        }

        if(Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int mousePosGrid = new Vector2Int(
                Mathf.FloorToInt(mousePos.y),
                Mathf.FloorToInt(mousePos.x)
                );
            if (!IsValidPosition(mousePosGrid)) return;
            if (currentCellFillValue == -1) return;
            centerObjects[currentCellFillValue].gameObject.SetActive(true);
            centerObjects[currentCellFillValue].transform.position = new Vector3(
                mousePosGrid.y + 0.5f,
                mousePosGrid.x + 0.5f,
                0
                );
            startCenters[currentCellFillValue] = mousePosGrid;
            EditorUtility.SetDirty(puzzleGame1LevelConfiguration);
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (currentCellFillValue == -1) return;
            BlockPiece spawnedPiece = GetBlockPiece();
            for(int i = 0; i< puzzleGame1LevelConfiguration.Blocks.Count; i++) 
            {
                if (puzzleGame1LevelConfiguration.Blocks[i].Id == spawnedPiece.Id)
                {
                    puzzleGame1LevelConfiguration.Blocks.RemoveAt(i);
                    i--;
                }
            }

            puzzleGame1LevelConfiguration.Blocks.Add(spawnedPiece);
            if (spawnedBlocks[currentCellFillValue] != null)
            {
                Destroy(spawnedBlocks[currentCellFillValue].gameObject);
            }
            spawnedBlocks[currentCellFillValue] = Instantiate(_blockPrefab);
            spawnedBlocks[currentCellFillValue].Init(spawnedPiece, startPos);
            EditorUtility.SetDirty(puzzleGame1LevelConfiguration);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveBlock(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveBlock(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            MoveBlock(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveBlock(Vector2Int.left);
        }
    }

    private void MoveBlock(Vector2Int offset)
    {
        for(int i=0; i< puzzleGame1LevelConfiguration.Blocks.Count; ++i) 
        {
            if (puzzleGame1LevelConfiguration.Blocks[i].Id == currentCellFillValue)
            {
                Vector2Int pos = puzzleGame1LevelConfiguration.Blocks[i].StartPos;
                pos.x += offset.x;
                pos.y += offset.y;
                BlockPiece piece = puzzleGame1LevelConfiguration.Blocks[i];   
                piece.StartPos = pos;
                puzzleGame1LevelConfiguration.Blocks[i] = piece;
                Vector3 movePos = spawnedBlocks[currentCellFillValue].transform.position;
                movePos.x += offset.y * _blockSpawnSize;
                movePos.y += offset.x * _blockSpawnSize;
                spawnedBlocks[currentCellFillValue].transform.position = movePos;
            }
        }
        EditorUtility.SetDirty(puzzleGame1LevelConfiguration);
    }
    private BlockPiece GetBlockPiece()
    {
        int id = currentCellFillValue;
        BlockPiece result = new BlockPiece();
        result.Id = id;
        result.CenterPos = startCenters[id];
        result.StartPos = Vector2Int.zero;
        result.BlockPositions = new List<Vector2Int>();
        for(int i=0; i< _rows; i++)
        {
            for(int j=0; j< _columns; j++)
            {
                if (gridCells[i, j].CellValue == id)
                {
                    result.BlockPositions.Add(new Vector2Int(i, j) - result.CenterPos);
                }
            }
        }
        return result;
    }

    private bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < _rows && pos.y < _columns;
    }

    public void ChangeCellFillValue(int value)
    {
        currentCellFillValue = value;
    }

}
#endif