using Infrastructure.States;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameBootstrapper : IStartable
    {
        private readonly GameStateMachine _gameStateMachine;

        [Inject]
        public GameBootstrapper(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Start()
        {
            _gameStateMachine.StartGame();
        }
    }
}