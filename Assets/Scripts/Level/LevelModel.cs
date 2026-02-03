using Block;
using UnityEngine;

namespace GameBoard.Level
{
    public struct LevelModel
    {
        public readonly Vector2Int GridSize;
        public readonly BlockType[] BlockTypes;

        public LevelModel(Vector2Int gridSize, BlockType[] blockTypes)
        {
            GridSize  = gridSize;
            BlockTypes = blockTypes;
        }
    }
}