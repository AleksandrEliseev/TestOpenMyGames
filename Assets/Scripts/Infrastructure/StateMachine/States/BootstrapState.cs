using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.GameModel;
using SaveLoadService;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameplaySignals _gameplaySignals;
        private readonly ILoadService _loadService;
        private readonly IGameplayModel _gameplayModel;
         
        [Inject]
        public BootstrapState(
            IGameplaySignals gameplaySignals,
            ILoadService loadService,
            IGameplayModel gameplayModel
        )
        {
            _gameplaySignals = gameplaySignals;
            _loadService = loadService;
            _gameplayModel = gameplayModel;
        }

        public async UniTask Enter(CancellationToken token)
        {
            Debug.Log("Bootstrap State Entered");
            _gameplayModel.CurrentLevel = _loadService.GetCurrentLevel();
            _gameplaySignals.RequestStateChange<LoadLevelState>();
        }

        public UniTask Exit(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}