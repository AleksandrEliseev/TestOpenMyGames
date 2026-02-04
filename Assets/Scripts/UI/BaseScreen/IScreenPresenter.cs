using System;

namespace UI.BaseScreen
{
    public interface IScreenPresenter : IDisposable
    {
        void Show();
        void Hide();
    }
}

