using System;
using Block;
using UnityEngine;

namespace Level
{
    [Serializable]
    public struct LevelModel
    {
        public Vector2Int GridSize;
        public BlockType[] BlockTypes;

        public LevelModel(Vector2Int gridSize, BlockType[] blockTypes)
        {
            GridSize  = gridSize;
            BlockTypes = blockTypes;
        }
    }
}