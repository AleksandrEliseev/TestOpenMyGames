using UI.BaseScreen;
using VContainer;
namespace UI.GameplayScreen
{
    public class GameplayScreenPresenterFactory : IPresenterFactory<IGameplayScreenView, GameplayScreenPresenter>
    {
        private readonly IObjectResolver _objectResolver;

        public GameplayScreenPresenterFactory(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public GameplayScreenPresenter Create(IGameplayScreenView view)
        {
            var presenter = new GameplayScreenPresenter(view);
            _objectResolver.Inject(presenter);
            return presenter;
        }

    }
}

