using GameCamera;
using UnityEngine;
using VContainer;

namespace GameBoard.Grid
{
    public interface IGridScaler
    {
        void Scale(GridModel grid, float topPadding, float bottomPadding, float sidePadding);
    }
    public class GridScaler: IGridScaler
    {
        private readonly CameraContainer _camera;
        private readonly Transform _gridTransform;

        [Inject]
        public GridScaler(
            CameraContainer camera,
            Transform gridTransform
        )
        {
            _camera = camera;
            _gridTransform = gridTransform;
        }

        public void Scale(GridModel grid, float topPadding, float bottomPadding, float sidePadding)
        {
            Camera camera = _camera.GameCamera;
            float screenHeight = camera.orthographicSize * 2;
            float screenWidth = screenHeight * camera.aspect;

            float availableHeight = screenHeight - (topPadding + bottomPadding);
            float availableWidth = screenWidth - (sidePadding * 2);

            float cellWidthForWidth = availableWidth / grid.Size.x;
            float cellHeightForHeight = availableHeight / grid.Size.y;

            float cellSize = Mathf.Min(cellWidthForWidth, cellHeightForHeight);

            grid.CellSize = new Vector2(cellSize, cellSize);

            float totalGridWidth = grid.Size.x * cellSize;

            float xOffset = (availableWidth - totalGridWidth) / 2;
            float yOffset =
                Mathf.Clamp(_gridTransform.position.y + screenHeight / 2f - bottomPadding - cellSize / 2f,
                    0.05f * screenHeight,
                    0.25f * screenHeight);


            Vector2 startPosition = new Vector2(
                -screenWidth / 2 + sidePadding + xOffset + cellSize / 2,
                -screenHeight / 2 + bottomPadding + yOffset + cellSize / 2
            );

            for (int y = 0; y < grid.Size.y; y++)
            {
                for (int x = 0; x < grid.Size.x; x++)
                {
                    int index = y * grid.Size.x + x;
                    GridCell cell = grid.Cells[index];

                    cell.WorldPosition = startPosition + new Vector2(x * cellSize, y * cellSize);
                }
            }
        }
    }
}