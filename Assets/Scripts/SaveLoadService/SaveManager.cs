using System;
using Level;
using SaveLoadService.Strategy;
using VContainer;

namespace SaveLoadService
{
    public interface ISaveManager
    {
        int GetCurrentLevel();
        bool TryToLoadLevel(int levelNumber, out SaveLevelData levelData);
        void ClearLevelData();
        void UpdateLevelData(SaveLevelData saveLevelData);
    }

    public class SaveManager : ISaveManager, IDisposable
    {
        private readonly ISaveStrategy _saveStrategy;

        private SaveLevelData _saveLevelData;

        [Inject]
        public SaveManager(ISaveStrategy saveStrategy)
        {
            _saveStrategy = saveStrategy;
            _saveLevelData = _saveStrategy.Load<SaveLevelData>(SaveKeysConst.LevelDataKey);
        }

        public int GetCurrentLevel()
        {
            return _saveLevelData == null ? 1 : _saveLevelData.LevelNumber;
        }
        public bool TryToLoadLevel(int levelNumber, out SaveLevelData levelData)
        {
            if (_saveLevelData != null && _saveLevelData.LevelNumber == levelNumber)
            {
                levelData = _saveLevelData;
                return true;
            }

            levelData = null;
            return false;
        }
        public void ClearLevelData()
        {
            _saveStrategy.Delete(SaveKeysConst.LevelDataKey);
            _saveLevelData = null;
        }
        public void UpdateLevelData(SaveLevelData saveLevelData)
        {
            _saveLevelData = saveLevelData;
            _saveStrategy.Save(SaveKeysConst.LevelDataKey, _saveLevelData);
        }
        public void Dispose()
        {
            _saveStrategy.Flush();
        }
    }

    [Serializable]
    public class SaveLevelData
    {
        public int LevelNumber;
        public LevelModel LevelModel;
    }
}