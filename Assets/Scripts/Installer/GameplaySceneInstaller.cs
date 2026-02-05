using Ballon;
using Ballon.Settings;
using Block;
using GameBoard.Configuration;
using GameBoard.Grid;
using GameBoard.Level.Settings;
using GameCamera;
using Grid.Settings;
using Infrastructure;
using Infrastructure.GameModel;
using Infrastructure.StateMachine.States;
using Infrastructure.States;
using Input;
using Mechanics;
using Level;
using SaveLoadService;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using UI.GameplayScreen;

namespace Installer
{
    //DI Instaler
    public class GameplaySceneInstaller : LifetimeScope
    {
        [SerializeField] private CameraContainer _cameraContainer;

        [SerializeField] private LevelTexturesDatabase _levelTexturesDatabase;
        [SerializeField] private BlockConfig _blockConfig;
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private BallonConfig _ballonConfig;
      

        [SerializeField] private Transform _gridTransform;
        [SerializeField] private Transform _blockPoolContainer;
        [SerializeField] private Transform _ballonPoolContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cameraContainer)
                .AsSelf();

            builder.Register<BallonSpawner>(Lifetime.Singleton)
                .WithParameter(_ballonConfig)
                .WithParameter(_ballonPoolContainer)
                .AsImplementedInterfaces();

            builder
                .Register<PrefsSaveLoadService>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<GameplaySignals>(Lifetime.Singleton)
                .As<IGameplaySignals>();

            builder.Register<GameplayModel>(Lifetime.Singleton)
                .As<IGameplayModel>();

            builder.Register<TextureLevelParserStrategy>(Lifetime.Singleton)
                .As<ILevelParser>()
                .WithParameter(_levelTexturesDatabase);

            builder.Register<GridScaler>(Lifetime.Singleton)
                .As<IGridScaler>()
                .WithParameter(_gridTransform);

            builder.Register<GridManager>(Lifetime.Singleton)
                .As<IGridManager>()
                .WithParameter(_gridConfig);

            builder.Register<MatchMechanic>(Lifetime.Singleton)
                .As<IMatchMechanic>();

            builder.Register<BlockFactory>(Lifetime.Singleton)
                .WithParameter("poolContainer", _blockPoolContainer)
                .WithParameter(_blockConfig)
                .AsImplementedInterfaces();


            builder.Register<InputSystem>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<SwapMechanic>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            #region StateMachine

            builder.Register<BootstrapState>(Lifetime.Singleton)
                .AsSelf();
            
            builder.Register<LoadLevelState>(Lifetime.Singleton)
                .AsSelf();
            
            builder.Register<GameLoopState>(Lifetime.Singleton)
                .AsSelf();
            
            builder.Register<LevelCompleteState>(Lifetime.Singleton)
                .AsSelf();

            builder.Register<GameStateMachine>(Lifetime.Singleton)
                .AsSelf();

            #endregion


            #region UI

            builder.Register<GameplayScreenPresenterFactory>(Lifetime.Singleton)
                .AsSelf();

            #endregion

            builder.RegisterEntryPoint<GameBootstrapper>();
        }
    }
}