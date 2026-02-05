using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachine.States;
using VContainer;

namespace Infrastructure.States
{
    public class GameStateMachine : IDisposable
    {
        private readonly Dictionary<Type, IState> _states;
        private readonly IGameplaySignals _gameplaySignals;
        private IState _activeState;
        
        CancellationTokenSource _cancellationTokenSource;

        [Inject]
        public GameStateMachine(IObjectResolver resolver, IGameplaySignals gameplaySignals)
        {
            _gameplaySignals = gameplaySignals;
            _states = new Dictionary<Type, IState>
            {
                [typeof(BootstrapState)] = resolver.Resolve<BootstrapState>(),
                [typeof(LoadLevelState)] = resolver.Resolve<LoadLevelState>(),
                [typeof(GameLoopState)] = resolver.Resolve<GameLoopState>(),
                [typeof(LevelCompleteState)] = resolver.Resolve<LevelCompleteState>(),
                [typeof(RestartLevelState)] = resolver.Resolve<RestartLevelState>()
            };

            _gameplaySignals.OnStateChangeRequested += ChangeState;
        }

        public void Dispose()
        {
            _gameplaySignals.OnStateChangeRequested -= ChangeState;
        }
        
        public void StartGame()
        {
            ChangeState(typeof(BootstrapState));
        }
        
        private void ChangeState(Type state)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            Enter(state, _cancellationTokenSource.Token).Forget();
        }

        private async UniTask Enter(Type stateType, CancellationToken token)
        {
            if (_activeState != null)
                await _activeState.Exit(token);

            IState state = _states[stateType];
            _activeState = state;

            await state.Enter(token);
        }
    }
}