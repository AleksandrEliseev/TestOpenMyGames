using System.Threading;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using Infrastructure.GameModel;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IState
    {
        private readonly IGameplaySignals _gameplaySignals;
        private readonly IGridManager _gridManager;
        private readonly IGameplayModel _gameplayModel;
        

        [Inject]
        public LoadLevelState(
            IGameplaySignals gameplaySignals,
            IGridManager gridManager,
            IGameplayModel gameplayModel
        )
        {
            _gameplaySignals = gameplaySignals;
            _gridManager = gridManager;
            _gameplayModel = gameplayModel;
        }

        public async UniTask Enter(CancellationToken token)
        {
            Debug.Log("Load Level State");
            _gridManager.GenerateGrid(_gameplayModel.CurrentLevel);
            _gameplaySignals.RequestStateChange<GameLoopState>();
        }

        public UniTask Exit(CancellationToken token)
        {
            _gameplaySignals.StartLevel();
            return UniTask.CompletedTask;
        }
    }
}