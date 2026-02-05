using System.Threading;
using Cysharp.Threading.Tasks;
using GameBoard.Grid;
using Infrastructure.GameModel;
using SaveLoadService;
using VContainer;

namespace Infrastructure.StateMachine.States
{
    public class LevelCompleteState : IState
    {
        private readonly IGameplaySignals _gameplaySignals;
        private readonly IGameplayModel _gameplayModel;
        private readonly ISaveService _saveService;
        private readonly IGridManager _gridManager;

        [Inject]
        public LevelCompleteState(
            IGameplaySignals gameplaySignals,
            IGameplayModel gameplayModel,
            IGridManager gridManager,
            ISaveService saveService
        )
        {
            _gameplaySignals = gameplaySignals;
            _gameplayModel = gameplayModel;
            _saveService = saveService;
        }

        public UniTask Enter(CancellationToken token)
        {
            _gameplayModel.CurrentLevel += 1;
            _gameplaySignals.RequestStateChange<LoadLevelState>();
            return UniTask.CompletedTask;
        }
        public UniTask Exit(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}