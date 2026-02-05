using VContainer;

namespace Infrastructure.GameModel
{
    public class GameplayModel : IGameplayModel
    {
        private readonly IGameplaySignals _signals;
        
        private int _currentLevel = 0;
        
        [Inject]
        public GameplayModel (IGameplaySignals signals)
        {
            _signals = signals;
        }
        
        public int CurrentLevel  
        {
            get => _currentLevel;
            set
            {
                if (_currentLevel != value)
                {
                    _currentLevel = value;
                    _signals.LevelChanged(value);
                }
            }
        }
    }

    public interface IGameplayModel
    {
        int CurrentLevel { get; set; }
    }
    
}