namespace Level
{
    public interface ILevelParser
    {
        public int LevelCount { get; }
        public LevelModel ParseLevel(int levelNumber);
    }
}