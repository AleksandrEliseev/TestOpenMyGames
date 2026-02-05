using Block;
using Grid.Settings;
using Level;
using SaveLoadService;
using UnityEngine;
using VContainer;


namespace GameBoard.Grid
{
    public interface IGridManager
    {
        void GenerateGrid(int levelNumber);
        void ClearGrid();
        GridModel Grid { get; }
        LevelModel GetLevelModelFromGrid();
        bool IsAllCellClear();
    }

    public class GridManager : IGridManager 
    {
        private readonly ILevelParser _levelParser;
        private readonly ILoadService _loadService;
        private readonly IBlockFactory _blockFactory;
        private readonly IGridScaler _gridScaler;
        private readonly GridConfig _gridConfig;

        public GridModel Grid { get; private set; }

        [Inject]
        public GridManager(
            ILevelParser levelParser,
            ILoadService loadService,
            IBlockFactory blockFactory,
            IGridScaler gridScaler,
            GridConfig gridConfig
        )
        {
            _levelParser = levelParser;
            _loadService = loadService;
            _blockFactory = blockFactory;
            _gridScaler = gridScaler;
            _gridConfig = gridConfig;
        }

        public void GenerateGrid(int levelNumber)
        {
            TryToCreateGridModel(levelNumber);
            GenerateBlocks();
        }

        private void GenerateBlocks()
        {
            BlockView blockView;
            foreach (var cell in Grid.Cells)
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
                    blockView.Init(cell.GridPosition);
                    blockView.SetPosition(cell.WorldPosition);
                    blockView.SetSize(Grid.CellSize);

                    cell.BlockView = blockView;
                }
            }
        }

        private void TryToCreateGridModel(int levelNumber)
        {
            LevelModel levelModel = _loadService.TryToLoadLevel(levelNumber, out SaveLevelData levelData)
                ? levelData.LevelModel 
                : _levelParser.ParseLevel(levelNumber);
            

            Vector2Int gridSize = new Vector2Int(levelModel.GridSize.x, levelModel.GridSize.y);
            
            var grid = new GridModel(gridSize);
            int cellCount = gridSize.x * gridSize.y;

            GridCell сell;
            for (int i = 0; i < cellCount; i++)
            {
                сell = new GridCell();
                сell.GridPosition = new Vector2Int(i % gridSize.x, i / gridSize.x);
                сell.BlockType = levelModel.BlockTypes[i];
                grid.Cells[i] = сell;
            }

            //Added neighbors           
            for (int i = 0; i < cellCount; i++)
            {
                сell = grid.Cells[i];
                int x = сell.GridPosition.x;
                int y = сell.GridPosition.y;

                // Left neighbor
                if (x > 0)
                    сell.LeftNeighbor = grid.Cells[i - 1];

                // Right neighbor
                if (x < gridSize.x - 1)
                    сell.RightNeighbor = grid.Cells[i + 1];

                // Bottom neighbor
                if (y > 0)
                    сell.BottomNeighbor = grid.Cells[i - gridSize.x];

                // Top neighbor
                if (y < gridSize.y - 1)
                    сell.TopNeighbor = grid.Cells[i + gridSize.x];
            }

            _gridScaler.Scale(grid, _gridConfig.TopPadding, _gridConfig.BottomPadding, _gridConfig.SidePadding);

            Grid = grid;
        }
        public void ClearGrid()
        {
            foreach (var gridCell in Grid.Cells)
            {
                if (gridCell.BlockView == null)
                    continue;

                _blockFactory.ReturnBlock(gridCell.BlockView);
                gridCell.BlockView = null;
            }
        }
        
        public LevelModel GetLevelModelFromGrid()
        {
            if (Grid == null) 
                return default;

           
            Vector2Int gridSize = new Vector2Int(Grid.Size.x, Grid.Size.y);

            var cells = Grid.Cells;
            BlockType[] blockTypes = new BlockType[cells.Length];
            
            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];
                blockTypes[i] = cell != null ? cell.BlockType : BlockType.None;
            }

            return  new LevelModel(gridSize, blockTypes);
        }
        public bool IsAllCellClear()
        {
            foreach (var cell in Grid.Cells)
            {
                if (cell.BlockType != BlockType.None)
                    return false;
            }

            return true;
        }
    }
}