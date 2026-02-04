using Block;
using GameBoard.Grid;
using Mechanics;
using UnityEngine;
using VContainer;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _levelNumber = 1;
    [SerializeField] private float _topPadding = 1.0f;
    [SerializeField] private float _bottomPadding = 1.0f;
    [SerializeField] private float _sidePadding = 0.5f;

    private GridManager _gridManager;
    private GridScaler _gridScaler;
    private BlockFactory _blockFactory;

    [Inject]
    public void Construct(GridManager gridManager, GridScaler gridScaler, BlockFactory blockFactory)
    {
        _gridManager = gridManager;
        _gridScaler = gridScaler;
        _blockFactory = blockFactory;
    }

    private void Start()
    {
        Debug.Log($"LevelGenerator: Generating level {_levelNumber}");
        
        GridModel grid = _gridManager.LoadLevel(_levelNumber);
        
        _gridScaler.Scale(grid, _topPadding, _bottomPadding, _sidePadding);

        GenerateBlocks(grid);
    }

    private void GenerateBlocks(GridModel grid)
    {
        BlockView blockView;
        foreach (var cell in grid.Cells)
        {
            if (cell.BlockType == BlockType.None)
            {
                blockView = null;
            }
            else
            {
                blockView = _blockFactory.GetBlock(cell.BlockType);
            }

            if (blockView != null)
            {
                blockView.Init(cell.GridPosition); // Init grid position
                blockView.SetPosition(cell.WorldPosition);
                blockView.SetSize(grid.CellSize);
                
                cell.BlockView = blockView;
            }
        }
    }
}