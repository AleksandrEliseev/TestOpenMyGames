using System.Collections.Generic;
using Block;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using UnityEngine;
using VContainer;

namespace Mechanics
{
    public class MatchMechanic
    {
        private readonly GridManager _gridManager;
        private readonly BlockFactory _blockFactory;

        [Inject]
        public MatchMechanic(GridManager gridManager, BlockFactory blockFactory)
        {
            _gridManager = gridManager;
            _blockFactory = blockFactory;
        }

        public async UniTask<bool> CheckAndProcessMatches(System.Threading.CancellationToken token)
        {
            var matches = FindMatches();

            if (matches.Count == 0)
                return false;

            await DestroyBlocks(matches, token);
            return true;
        }

        private HashSet<BlockView> FindMatches()
        {
            var matchedBlocks = new HashSet<BlockView>();
            var grid = _gridManager.Grid;
            int width = grid.Size.x;
            int height = grid.Size.y;

            // Horizontal checks
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 2; x++)
                {
                    BlockView b1 = GetBlock(x, y);
                    BlockView b2 = GetBlock(x + 1, y);
                    BlockView b3 = GetBlock(x + 2, y);

                    if (b1 != null && b2 != null && b3 != null &&
                        b1.Type == b2.Type && b1.Type == b3.Type)
                    {
                        matchedBlocks.Add(b1);
                        matchedBlocks.Add(b2);
                        matchedBlocks.Add(b3);
                    }
                }
            }

            // Vertical checks
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 2; y++)
                {
                    BlockView b1 = GetBlock(x, y);
                    BlockView b2 = GetBlock(x, y + 1);
                    BlockView b3 = GetBlock(x, y + 2);

                    if (b1 != null && b2 != null && b3 != null &&
                        b1.Type == b2.Type && b1.Type == b3.Type)
                    {
                        matchedBlocks.Add(b1);
                        matchedBlocks.Add(b2);
                        matchedBlocks.Add(b3);
                    }
                }
            }

            return matchedBlocks;
        }

        private BlockView GetBlock(int x, int y)
        {
            int index = y * _gridManager.Grid.Size.x + x;
            if (index >= 0 && index < _gridManager.Grid.Cells.Length)
            {
                return _gridManager.Grid.Cells[index].BlockView;
            }
            return null;
        }

        private async UniTask DestroyBlocks(HashSet<BlockView> blocks, System.Threading.CancellationToken token)
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (var block in blocks)
            {
                // Clear grid reference
                int index = block.GridPosition.y * _gridManager.Grid.Size.x + block.GridPosition.x;
                GridCell cell = _gridManager.Grid.Cells[index];
                
                cell.BlockView = null;
                cell.BlockType = BlockType.None;

                tasks.Add(DestroyBlockAnimation(block, token));
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask DestroyBlockAnimation(BlockView block, System.Threading.CancellationToken token)
        {
            if (block != null)
            {
                await block.AnimateDestruction(token);
                _blockFactory.ReturnBlock(block);
            }
        }
    }
}
