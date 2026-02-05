using System;
using TMPro;
using UI.BaseScreen;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.GameplayScreen
{
    public interface IGameplayScreenView : IBaseScreenView
    {
        event Action OnRestartButtonClicked;
        event Action OmNextLevelButtonClicked;
        void SetLevelText(int level);
        void SetActiveNextLevelButton(bool isActive);
        void SetActiveRestartButton(bool isActive);
    }

    public class GameplayScreenView : BaseScreenView, IGameplayScreenView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TMP_Text _levelText;
        
        public event Action OnRestartButtonClicked;
        public event Action OmNextLevelButtonClicked;

        private GameplayScreenPresenter _presenter;

        [Inject]
        public void Construct(GameplayScreenPresenterFactory factory)
        {
            _presenter = factory.Create(this);
            _presenter.Show();
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }

        public void SetLevelText(int level)
        {
            _levelText.text = $"Level {level}";
        }
        public void SetActiveNextLevelButton(bool isActive)
        {
            _nextLevelButton.gameObject.SetActive(isActive);
        }
        public void SetActiveRestartButton(bool isActive)
        {
            _restartButton.gameObject.SetActive(isActive);
        }
        
        private void RestartButtonClicked()
        {
            OnRestartButtonClicked?.Invoke();
        }
        private void NextLevelButtonClicked()
        {           
            OmNextLevelButtonClicked?.Invoke();
        }
        protected override void OnEnable()
        {
            _restartButton.onClick.AddListener(RestartButtonClicked);
            _nextLevelButton.onClick.AddListener(NextLevelButtonClicked);
        }
        protected override void OnDisable()
        {
            _restartButton.onClick.RemoveListener(RestartButtonClicked);
            _nextLevelButton.onClick.RemoveListener(NextLevelButtonClicked);
        }
    }
}