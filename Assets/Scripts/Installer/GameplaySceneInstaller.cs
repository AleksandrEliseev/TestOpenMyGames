using DefaultNamespace;
using GameBoard.Configuration;
using GameBoard.Grid;
using GameBoard.Level;
using GameBoard.Level.Settings;
using Input;
using GameBoard.Mechanics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installer
{
    //DI Instaler
    public class GameplaySceneInstaller : LifetimeScope
    {
        [SerializeField] private CameraContainer _cameraContainer;
        [SerializeField] private LevelTexturesDatabase _levelTexturesDatabase;
        [SerializeField] private BlockConfig _blockConfig;
        
        [SerializeField] private Transform _gridTransform;
        [SerializeField] private Transform _blockPoolContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cameraContainer)
                .AsSelf();
            
            builder.Register<TextureLevelParserStrategy>(Lifetime.Singleton)
                .As<ILevelParser>()
                .WithParameter(_levelTexturesDatabase);
            
            builder.Register<GridScaler>(Lifetime.Singleton)
                .AsSelf()
                .WithParameter(_gridTransform);
            
            builder.Register<GridManager>(Lifetime.Singleton)
                .AsSelf();

            builder.Register<MatchMechanic>(Lifetime.Singleton)
                .AsSelf();

            builder.Register<BlockFactory>(Lifetime.Singleton)
                .AsSelf()
                .WithParameter("poolContainer", _blockPoolContainer)
                .WithParameter(_blockConfig)
                .AsImplementedInterfaces();

            builder.Register<InputSystem>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder.Register<SwapMechanic>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }
    }
}