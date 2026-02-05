using System;
using System.Collections.Generic;
using Block;
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

        private readonly Dictionary<BlockType, Queue<BlockView>> _pools = new Dictionary<BlockType, Queue<BlockView>>();
        
        private const int InitialPoolSizePerType = 10;
        
        public BlockFactory(Transform poolContainer, BlockConfig blockConfig)
        {
            _poolContainer = poolContainer;
            _blockConfig = blockConfig;
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
            if (!_pools.TryGetValue(type, out var pool))
            {
                pool = new Queue<BlockView>();
                _pools[type] = pool;
            }

            if (pool.Count > 0)
            {
                BlockView block = pool.Dequeue();
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
            
            if (!_pools.TryGetValue(block.Type, out var pool))
            {
                pool = new Queue<BlockView>();
                _pools[block.Type] = pool;
            }
            
            pool.Enqueue(block);
        }

        private BlockView CreateNewBlock(BlockType type)
        {
            BlockView prefab = _blockConfig.GetBlockPrefab(type);
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
