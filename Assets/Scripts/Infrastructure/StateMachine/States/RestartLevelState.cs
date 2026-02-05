
using System.Threading;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using Infrastructure.GameModel;
using Mechanics;
using SaveLoadService;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine.States
{
    public class RestartLevelState : IState
    {
        private readonly ISaveService _saveService;
        private readonly IGameplaySignals _gameplaySignals;
        private readonly ISwapMechanic _swapMechanic;
        private readonly IGridManager _gridManager;

        [Inject]
        public RestartLevelState(
            ISaveService saveService,
            IGameplaySignals gameplaySignals,
            ISwapMechanic swapMechanic,
            IGridManager gridManager
        )
        {
            _saveService = saveService;
            _gameplaySignals = gameplaySignals;
            _swapMechanic = swapMechanic;
            _gridManager = gridManager;
        }

        public async UniTask Enter(CancellationToken token)
        {
            Debug.Log("Restart Level State Entered");
            _swapMechanic.DestroyAnimations();
            _gridManager.ClearGrid();
            _saveService.ClearLevelData();
            _gameplaySignals.RequestStateChange<LoadLevelState>();
        }
        public UniTask Exit(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}