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
        private readonly ISaveManager _saveManager;
        private readonly IGameplayModel _gameplayModel;
         
        [Inject]
        public BootstrapState(
            IGameplaySignals gameplaySignals,
            ISaveManager saveManager,
            IGameplayModel gameplayModel
        )
        {
            _gameplaySignals = gameplaySignals;
            _saveManager = saveManager;
            _gameplayModel = gameplayModel;
        }

        public async UniTask Enter(CancellationToken token)
        {
            Debug.Log("Bootstrap State Entered");
            _gameplayModel.CurrentLevel = _saveManager.GetCurrentLevel();
            _gameplaySignals.RequestStateChange<LoadLevelState>();
        }

        public UniTask Exit(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}