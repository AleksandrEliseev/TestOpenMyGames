using System.Threading;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using Infrastructure.GameModel;
using Input;
using Mechanics;
using SaveLoadService;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IInputSystem _inputSystem;
        private readonly ISwapMechanic _swapMechanic;
        private readonly IGameplayModel _gameplayModel;
        private readonly ISaveManager _saveManager;
        private readonly IGridManager _gridManager;
        private readonly IGameplaySignals _gameplaySignals;

   
        [Inject]
        public GameLoopState( 
            IInputSystem inputSystem,
            ISwapMechanic swapMechanic,
            IGameplayModel gameplayModel,
            ISaveManager saveManager,
            IGridManager gridManager,
            IGameplaySignals gameplaySignals
            )
        {
            _inputSystem = inputSystem;
            _swapMechanic = swapMechanic;
            _gameplayModel = gameplayModel;
            _saveManager = saveManager;
            _gridManager = gridManager;
            _gameplaySignals = gameplaySignals;
        }
             
      


        public UniTask Enter(CancellationToken token)
        {
            Debug.Log("Game Loop State Entered");
            
            _swapMechanic.OnSwapEnded += OnSwapEnded;
            _inputSystem.IsInputEnabled = true;
            _swapMechanic.CheckLevelAfterLoadOrRestart();
            
            return UniTask.CompletedTask;
        }

        public UniTask Exit(CancellationToken token)
        {
            _swapMechanic.OnSwapEnded -= OnSwapEnded;
            _swapMechanic.DestroyAnimations();
            _gridManager.ClearGrid();
            _saveManager.ClearLevelData();
            return UniTask.CompletedTask;
        }

        private void OnSwapEnded()
        {
            _saveManager.UpdateLevelData(new SaveLevelData()
            {
                LevelNumber = _gameplayModel.CurrentLevel,
                LevelModel = _gridManager.GetLevelModelFromGrid()
            });

            if (_gridManager.IsAllCellClear())
            {
                _inputSystem.IsInputEnabled = false;
                _gameplaySignals.RequestStateChange<LevelCompleteState>();
            }
        }
    }
}