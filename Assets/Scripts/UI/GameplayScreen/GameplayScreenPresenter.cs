using GameBoard.Grid;
using UI.BaseScreen;
using UnityEngine;
using VContainer;

namespace UI.GameplayScreen
{
    public class GameplayScreenPresenter : BaseScreenPresenter<IGameplayScreenView>
    {
        public GameplayScreenPresenter(IGameplayScreenView view) : base(view)
        {
            Debug.Log("GameplayScreenPresenter created");
        }

        [Inject]
        public void Construct()
        {
           Debug.Log("GameplayScreenPresenter constructed with DI");
        }

        public override void Show()
        {
            View.OnRestartButtonClicked += OnRestartClicked;
            View.OmNextLevelButtonClicked += OnNextLevelClicked;
            
            View.ShowScreen();
            
            // Тестовая инициализация
            View.SetLevelText(1);
        }

        public override void Hide()
        {
            View.OnRestartButtonClicked -= OnRestartClicked;
            View.OmNextLevelButtonClicked -= OnNextLevelClicked;
            
            View.HideScreen();
        }

        private void OnRestartClicked()
        {
            Debug.Log("Restart clicked");
        }

        private void OnNextLevelClicked()
        {
            Debug.Log("Next level clicked");
        }
    }
}

