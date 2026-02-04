using Block;
using GameBoard.Grid;
using GameBoard.Level.Settings;
using Mechanics;
using UnityEngine;
using VContainer;

public interface IGridGenerator
{
}

public class GridGenerator
{
    private readonly GridManager _gridManager;
    private readonly GridScaler _gridScaler;
    private readonly BlockFactory _blockFactory;
    private readonly GridConfig _gridConfig;


    [Inject]
    public GridGenerator(
        GridManager gridManager,
        GridScaler gridScaler,
        BlockFactory blockFactory,
        GridConfig gridConfig
    )
    {
        _gridManager = gridManager;
        _gridScaler = gridScaler;
        _blockFactory = blockFactory;
        _gridConfig = gridConfig;
    }

    public void GenerateGrid(int levelNumber)
    {
        Debug.Log($"LevelGenerator: Generating level {levelNumber}");

        GridModel grid = _gridManager.LoadLevel(levelNumber);

        _gridScaler.Scale(grid, _gridConfig.TopPadding, _gridConfig.BottomPadding, _gridConfig.SidePadding);

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