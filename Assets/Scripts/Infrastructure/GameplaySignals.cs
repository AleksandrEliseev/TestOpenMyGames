using System;
using Infrastructure.StateMachine.States;

namespace Infrastructure
{
    public interface IGameplaySignals
    {
        event Action<int> OnLevelChanged;
        event Action<Type> OnStateChangeRequested;
        
        void LevelChanged(int levelIndex);
        void RequestStateChange<TState>() where TState : IState;
    }

    public class GameplaySignals : IGameplaySignals
    {
        public event Action<int> OnLevelChanged;
        public event Action<Type> OnStateChangeRequested;

        public void LevelChanged(int levelIndex)
        {
            OnLevelChanged?.Invoke(levelIndex);
        }
        public void RequestStateChange<TState>() where TState : IState
        {
            OnStateChangeRequested?.Invoke(typeof(TState));
        }
    }
}