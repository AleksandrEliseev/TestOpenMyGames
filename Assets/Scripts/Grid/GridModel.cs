using UnityEngine;

namespace GameBoard.Grid
{
    public class GridModel
    {
        public GridCell[] Cells;
        public Vector2Int Size;
        public Vector2 CellSize;

        public GridModel(Vector2Int size)
        {
            Size = size;
            Cells = new GridCell[size.x * size.y];
        }
    }
}
