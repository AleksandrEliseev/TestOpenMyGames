namespace UI.BaseScreen
{
    public abstract class BaseScreenPresenter<TView> : IScreenPresenter where TView : class
    {
        protected readonly TView View;

        protected BaseScreenPresenter(TView view)
        {
            View = view;
        }

        public virtual void Show()
        {
            
        }

        public virtual void Hide()
        {
           
        }

        public virtual void Dispose()
        {
            Hide();
        }
    }
}

