using System;
using System.Collections.Generic;
using System.Threading;
using Block;
using Core;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameBoard.Mechanics
{
    public class SwapMechanic : IStartable, IDisposable
    {
        private readonly GridManager _gridManager;
        private readonly IInputSystem _inputSystem;
        private CancellationTokenSource _cts;
        private bool _isAnimating;

        private const float MoveDuration = 0.2f;

        [Inject]
        public SwapMechanic(GridManager gridManager, IInputSystem inputSystem)
        {
            _gridManager = gridManager;
            _inputSystem = inputSystem;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _inputSystem.OnSwipe += TrySwap;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _inputSystem.OnSwipe -= TrySwap;
        }

        private void TrySwap(BlockView block, Direction direction)
        {
            if (_isAnimating) 
                return;
            
            SwapAnimation(block,direction, _cts.Token).Forget();
        }

        private async UniTaskVoid SwapAnimation(BlockView block, Direction direction, CancellationToken token)
        {
            GridCell startCell = GetCellAt(block.GridPosition);
            if (startCell == null) 
                return;

            GridCell targetCell = GetTargetCell(startCell, direction);
            if (targetCell == null) 
                return;
            
            _isAnimating = true;

            try
            {
                if (targetCell.BlockView != null)
                {
                    await SwapBlocks(startCell, targetCell, token);
                }
                else
                {
                    await MoveBlock(startCell, targetCell, token);
                }
            }
            finally
            {
                _isAnimating = false;
            }
        }
        
        private GridCell GetCellAt(Vector2Int position)
        {
            if (_gridManager.Grid == null) return null;
            
            int index = position.y * _gridManager.Grid.Size.x + position.x;
            if (index >= 0 && index < _gridManager.Grid.Cells.Length)
            {
                return _gridManager.Grid.Cells[index];
            }
            return null;
        }

        private GridCell GetTargetCell(GridCell startCell, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return startCell.TopNeighbor;
                case Direction.Down: return startCell.BottomNeighbor;
                case Direction.Left: return startCell.LeftNeighbor;
                case Direction.Right: return startCell.RightNeighbor;
                default: return null;
            }
        }

        private async UniTask SwapBlocks(GridCell cell1, GridCell cell2, CancellationToken token)
        {
            BlockView block1 = cell1.BlockView;
            BlockView block2 = cell2.BlockView;
            
            UpdateCellData(cell1, block2);
            UpdateCellData(cell2, block1);
            
            await UniTask.WhenAll(
                AnimateMove(block1, cell2, token),
                AnimateMove(block2, cell1, token)
            );
        }

        private async UniTask MoveBlock(GridCell fromCell, GridCell toCell, CancellationToken token)
        {
            BlockView block = fromCell.BlockView;
            
            UpdateCellData(toCell, block);
            UpdateCellData(fromCell, null);
            
            await AnimateMove(block, toCell, token);            
        }

        private void UpdateCellData(GridCell cell, BlockView block)
        {
            cell.BlockView = block;
            cell.BlockType = block != null ? block.Type : BlockType.None;
        }

        private UniTask AnimateMove(BlockView block, GridCell targetCell, CancellationToken token)
        {
            if (block == null)
                return UniTask.CompletedTask;

            block.UpdateGridPosition(targetCell.GridPosition);
            return block.MoveTo(targetCell.WorldPosition, MoveDuration, token);
        }
    }
}
