using UnityEngine;

namespace UI.BaseScreen
{
    public interface IBaseScreenView
    {
        void ShowScreen();
        void HideScreen();
    }
    public abstract class BaseScreenView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        public void ShowScreen()
        {
            enabled = true;
            _canvas.enabled = true;
        }
        public void HideScreen()
        {
            _canvas.enabled = false;
            enabled = false;
        }
        protected abstract void OnEnable();
        protected abstract void OnDisable();
    }
}