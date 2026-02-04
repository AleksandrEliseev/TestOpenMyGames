using System;
using System.Collections.Generic;
using Block;
using UnityEngine;

namespace GameBoard.Configuration
{
    [CreateAssetMenu(fileName = "BlockConfig", menuName = "Configs/BlockConfig")]
    public class BlockConfig : ScriptableObject
    {
        [SerializeField] private List<BlockConfigData> _blockConfigs;
       
        public BlockView GetBlockPrefab(BlockType type)
        {
            foreach (var config in _blockConfigs)
            {
                if (config.BlockType == type)
                {
                    return config.BlockPrefab;
                }
            }
            throw new Exception($"Block prefab for type {type} not found in BlockConfig.");
        }
    }

    [Serializable]
    public class BlockConfigData
    {
        public BlockType BlockType;
        public BlockView BlockPrefab;
    }
}

