using Block;
using UnityEngine;

namespace GameBoard.Grid
{
    public class GridCell
    {
        public Vector2Int GridPosition = Vector2Int.zero;
        public Vector2 WorldPosition = Vector2.zero;
        public BlockType BlockType = BlockType.None;
        
        public GridCell LeftNeighbor;
        public GridCell RightNeighbor;
        public GridCell TopNeighbor;
        public GridCell BottomNeighbor;
    }
}