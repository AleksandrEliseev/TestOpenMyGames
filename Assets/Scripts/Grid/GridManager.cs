using Level;
using SaveLoadService;
using UnityEngine;
using VContainer;

namespace GameBoard.Grid
{
    public class GridManager
    {
        private readonly ILevelParser _levelParser;
        private readonly ILoadService _loadService;
        public GridModel Grid { get; private set; }

        [Inject]
        public GridManager(
            ILevelParser levelParser,
            ILoadService loadService
        )
        {
            _levelParser = levelParser;
            _loadService = loadService;
        }

        public GridModel LoadLevel(int levelNumber)
        {
            if (_loadService.TryToLoadLevel(levelNumber, out LevelModel levelModel))
            {
                Debug.Log($"Loading level {levelNumber}");
            }
            else
            {
                levelModel = _levelParser.ParseLevel(levelNumber);
                Debug.Log($"Parsed level {levelNumber} from resources");
            }

            Vector2Int gridSize = new Vector2Int(levelModel.GridSize.x, levelModel.GridSize.y);

            Grid = new GridModel(gridSize);
            int cellCount = gridSize.x * gridSize.y;

            GridCell сell;
            for (int i = 0; i < cellCount; i++)
            {
                сell = new GridCell();
                сell.GridPosition = new Vector2Int(i % gridSize.x, i / gridSize.x);
                сell.BlockType = levelModel.BlockTypes[i];
                Grid.Cells[i] = сell;
            }

            //Added neighbors           
            for (int i = 0; i < cellCount; i++)
            {
                сell = Grid.Cells[i];
                int x = сell.GridPosition.x;
                int y = сell.GridPosition.y;

                // Left neighbor
                if (x > 0)
                    сell.LeftNeighbor = Grid.Cells[i - 1];

                // Right neighbor
                if (x < gridSize.x - 1)
                    сell.RightNeighbor = Grid.Cells[i + 1];

                // Bottom neighbor
                if (y > 0)
                    сell.BottomNeighbor = Grid.Cells[i - gridSize.x];

                // Top neighbor
                if (y < gridSize.y - 1)
                    сell.TopNeighbor = Grid.Cells[i + gridSize.x];
            }

            return Grid;
        }
    }
}