using System;
using System.Collections.Generic;
using GameBoard.Configuration;
using UnityEngine;
using Object = UnityEngine.Object;
using VContainer.Unity;

namespace Block
{
    public interface IBlockFactory
    {
        BlockView GetBlock(BlockType type);
        void ReturnBlock(BlockView block);
    }
    public class BlockFactory : IStartable, IBlockFactory
    {
        private readonly Transform _poolContainer;
        private readonly BlockConfig _blockConfig;

        private readonly Queue<BlockView> _pools ;
     
        
        private const int InitialPoolSizePerType = 30;
        
        public BlockFactory(Transform poolContainer, BlockConfig blockConfig)
        {
            _poolContainer = poolContainer;
            _blockConfig = blockConfig;
            _pools = new Queue<BlockView>(InitialPoolSizePerType);
        }

        public void Start()
        {
            Prewarm(InitialPoolSizePerType);
        }

        private void Prewarm(int countPerType)
        {
            foreach (BlockType type in Enum.GetValues(typeof(BlockType)))
            {
                if (type == BlockType.None) continue;

                for (int i = 0; i < countPerType; i++)
                {
                    BlockView block = CreateNewBlock(type);
                    if (block != null)
                    {
                        ReturnBlock(block);
                    }
                }
            }
            Debug.Log("BlockFactory: Prewarm completed.");
        }

        public BlockView GetBlock(BlockType type)
        {
            if (_pools.Count > 0)
            {
                BlockView block = _pools.Dequeue();
                block.Initialize(type, _blockConfig.GetConfigByType(type).Animator);
                block.gameObject.SetActive(true);
                return block;
            }

            return CreateNewBlock(type);
        }

        public void ReturnBlock(BlockView block)
        {
            block.gameObject.SetActive(false);
            block.transform.SetParent(_poolContainer);
            block.transform.localScale = Vector3.one;
            
            _pools.Enqueue(block);
        }

        private BlockView CreateNewBlock(BlockType type)
        {
            BlockView prefab = _blockConfig.BlockPrefab;
            if (prefab == null)
            {
                Debug.LogError($"No prefab found for block type: {type}");
                return null;
            }

            BlockView newBlock = Object.Instantiate(prefab, _poolContainer);
            return newBlock;
        }
    }
}
