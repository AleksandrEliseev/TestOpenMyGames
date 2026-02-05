
using Infrastructure;
using Infrastructure.GameModel;
using Infrastructure.StateMachine.States;
using UI.BaseScreen;
using UnityEngine;
using VContainer;

namespace UI.GameplayScreen
{
    public class GameplayScreenPresenter : BaseScreenPresenter<IGameplayScreenView>
    {
        private IGameplaySignals _gameplaySignals;
       

        public GameplayScreenPresenter(IGameplayScreenView view) : base(view)
        {
        }

        [Inject]
        public void Construct(
            IGameplaySignals gameplaySignals
        )
        {
            _gameplaySignals = gameplaySignals;

            View.OnRestartButtonClicked += OnRestartClicked;
            View.OmNextLevelButtonClicked += OnNextLevelClicked;
            
            _gameplaySignals.OnLevelChanged += SetLevelNumber;
        }
      
        private void SetLevelNumber(int levelNumber)
        {
            View.SetLevelText(levelNumber);
        }
      

        public override void Show()
        {
            View.ShowScreen();
        }

        public override void Hide()
        {
            View.HideScreen();
        }

        private void OnRestartClicked()
        {
            Debug.Log("Restart clicked");
           _gameplaySignals.RequestStateChange<LoadLevelState>();
        }

        private void OnNextLevelClicked()
        {
            Debug.Log("Next level clicked");
            _gameplaySignals.RequestStateChange<LevelCompleteState>();
        }
        public override void Dispose()
        {
            base.Dispose();
            if (View != null)
            {
                View.OnRestartButtonClicked -= OnRestartClicked;
                View.OmNextLevelButtonClicked -= OnNextLevelClicked;
            }

            if (_gameplaySignals != null)
            {
                _gameplaySignals.OnLevelChanged -= SetLevelNumber;
            }
        }
    }
}