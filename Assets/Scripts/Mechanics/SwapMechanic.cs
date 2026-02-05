using System;
using System.Collections.Generic;
using System.Threading;
using Block;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using Infrastructure;
using Infrastructure.GameModel;
using Input;
using SaveLoadService;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Mechanics
{
    public interface ISwapMechanic
    {
        void CheckLevelAfterLoadOrRestart();
        void DestroyAnimations();
        event Action OnSwapEnded;
    }

    public class SwapMechanic : ISwapMechanic, IStartable, IDisposable
    {
        private readonly IGridManager _gridManager;
        private readonly IInputSystem _inputSystem;
        private readonly IMatchMechanic _matchMechanic;
        private readonly ISaveService _saveService;
        private readonly IGameplayModel _gameplayModel;
        private readonly IGameplaySignals _gameplaySignals;

        private CancellationTokenSource _cts;
        private bool _isAnimating;

        private const float MoveDuration = 0.2f;

        public event Action OnSwapEnded;

        [Inject]
        public SwapMechanic(
            IGridManager gridManager,
            IInputSystem inputSystem,
            IMatchMechanic matchMechanic,
            ISaveService saveService,
            IGameplayModel gameplayModel,
            IGameplaySignals gameplaySignals
        )
        {
            _gridManager = gridManager;
            _inputSystem = inputSystem;
            _matchMechanic = matchMechanic;
            _saveService = saveService;
            _gameplayModel = gameplayModel;
            _gameplaySignals = gameplaySignals;
        }

        public void Start()
        {
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

            if (direction == Direction.Up)
            {
                GridCell startCell = GetCellAt(block.GridPosition);
                if (startCell != null)
                {
                    GridCell targetCell = GetTargetCell(startCell, direction);
                    if (targetCell != null && targetCell.BlockView == null)
                    {
                        return;
                    }
                }
            }

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            SwapAnimation(block, direction, _cts.Token).Forget();
        }
        public void DestroyAnimations()
        {
            _cts?.Cancel();
        }
        
        public void CheckLevelAfterLoadOrRestart()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            CheckLevelAfterLoadOrRestart(_cts.Token).Forget();
        }

        private async UniTaskVoid CheckLevelAfterLoadOrRestart(CancellationToken token)
        {
            _isAnimating = true;
            try
            {
                await ProcessGravityAndMatches(token);
            }
            finally
            {
                OnSwapEnded?.Invoke();
                _isAnimating = false;
            }
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

                await ProcessGravityAndMatches(token);
            }
            finally
            {
                OnSwapEnded?.Invoke();
                _isAnimating = false;
            }
        }

        private async UniTask ProcessGravityAndMatches(CancellationToken token)
        {
            while (true)
            {
                await ApplyGravity(token);

                bool hasMatches = await _matchMechanic.CheckAndProcessMatches(token);

                if (!hasMatches)
                    break;
            }
        }

        private async UniTask ApplyGravity(CancellationToken token)
        {
            var grid = _gridManager.Grid;
            List<UniTask> gravityTasks = new List<UniTask>();

            for (int x = 0; x < grid.Size.x; x++)
            {
                int writeY = 0;
                for (int y = 0; y < grid.Size.y; y++)
                {
                    GridCell currentCell = GetCellAt(new Vector2Int(x, y));

                    if (currentCell.BlockView != null)
                    {
                        if (y != writeY)
                        {
                            GridCell targetCell = GetCellAt(new Vector2Int(x, writeY));

                            UpdateCellData(targetCell, currentCell.BlockView);
                            UpdateCellData(currentCell, null);

                            gravityTasks.Add(AnimateMove(targetCell.BlockView, targetCell, true, token));
                        }

                        writeY++;
                    }
                }
            }

            if (gravityTasks.Count > 0)
            {
                await UniTask.WhenAll(gravityTasks);
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
                AnimateMove(block1, cell2, false, token),
                AnimateMove(block2, cell1, false, token)
            );
        }

        private async UniTask MoveBlock(GridCell fromCell, GridCell toCell, CancellationToken token)
        {
            BlockView block = fromCell.BlockView;

            UpdateCellData(toCell, block);
            UpdateCellData(fromCell, null);

            await AnimateMove(block, toCell, false, token);
        }

        private void UpdateCellData(GridCell cell, BlockView block)
        {
            cell.BlockView = block;
            cell.BlockType = block != null ? block.Type : BlockType.None;
        }

        private UniTask AnimateMove(BlockView block, GridCell targetCell, bool isDown, CancellationToken token)
        {
            if (block == null)
                return UniTask.CompletedTask;

            float distance = Vector2.Distance(block.GridPosition, targetCell.GridPosition);
            float duration = MoveDuration * distance;

            block.UpdateGridPosition(targetCell.GridPosition);
            return block.MoveTo(targetCell.WorldPosition, duration, isDown, token);
        }
    }
}