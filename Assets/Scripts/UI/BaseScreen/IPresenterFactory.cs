namespace UI.BaseScreen
{
    public interface IPresenterFactory<TView, TPresenter> 
        where TPresenter : IScreenPresenter
    {
        TPresenter Create(TView view);
    }
}

