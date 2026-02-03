using Block;
using GameBoard.Grid;
using UnityEngine;
using VContainer;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private BlockView _fireBlockPrefab;
    [SerializeField] private BlockView _waterBlockPrefab;

    [SerializeField] private int _levelNumber = 1;
    [SerializeField] private float _topPadding = 1.0f;
    [SerializeField] private float _bottomPadding = 1.0f;
    [SerializeField] private float _sidePadding = 0.5f;

    private GridManager _gridManager;
    private GridScaler _gridScaler;

    [Inject]
    public void Construct(GridManager gridManager,GridScaler gridScaler)
    {
        _gridManager = gridManager;
        _gridScaler = gridScaler;   
    }

    private void Start()
    {
        GridModel grid = _gridManager.LoadLevel(_levelNumber);
        
        _gridScaler.Scale(grid, _topPadding, _bottomPadding, _sidePadding);

        GenerateBlocks(grid);
    }

    private void GenerateBlocks(GridModel grid)
    {
        BlockView blockView;
        foreach (var cell in grid.Cells)
        {
            switch (cell.BlockType)
            {
                case BlockType.Fire:
                    blockView = Instantiate(_fireBlockPrefab);
                    break;
                case BlockType.Water:
                    blockView = Instantiate(_waterBlockPrefab);
                    break;
                default:
                    blockView = null;
                    break;
            }
            if (blockView != null)
            {
                blockView.SetPosition(cell.WorldPosition);
                blockView.SetSize(grid.CellSize);
            }
        }
    }
}