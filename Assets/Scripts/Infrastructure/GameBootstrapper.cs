using Infrastructure.States;
using UnityEngine;
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
            #if UNITY_ANDROID
            Application.targetFrameRate = 60;
            #endif
            
            _gameStateMachine.StartGame();
        }
    }
}