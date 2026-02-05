using System;
using System.Collections.Generic;
using Block;
using UnityEngine;

namespace GameBoard.Configuration
{
    [CreateAssetMenu(fileName = "BlockConfig", menuName = "Configs/BlockConfig")]
    public class BlockConfig : ScriptableObject
    {
        [SerializeField] private BlockView _blockPrefab;

        [SerializeField] private List<BlockConfigData> _blockConfigs;

        public BlockView BlockPrefab => _blockPrefab;


        public BlockConfigData GetConfigByType(BlockType type)
        {
            foreach (var config in _blockConfigs)
            {
                if (config.BlockType == type)
                {
                    return config;
                }
            }

            Debug.LogWarning($"BlockConfig: No animator found for BlockType {type}");
            return null;
        }
    }

    [Serializable]
    public class BlockConfigData
    {
        public BlockType BlockType;
        public AnimatorOverrideController Animator;
        public Sprite InitialSprite;
    }
}