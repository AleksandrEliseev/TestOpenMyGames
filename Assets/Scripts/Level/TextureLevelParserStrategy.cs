using Block;
using GameBoard.Level.Settings;
using UnityEngine;
using VContainer;

namespace Level
{
    public class TextureLevelParserStrategy : ILevelParser
    {
        private readonly LevelTexturesDatabase _levelTexturesDatabase;

        public int LevelCount { get; private set; }

        [Inject]
        public TextureLevelParserStrategy(LevelTexturesDatabase levelTexturesDatabase)
        {
            _levelTexturesDatabase = levelTexturesDatabase;
            LevelCount = _levelTexturesDatabase.LevelCount;
        }

        public LevelModel ParseLevel(int levelNumber)
        {
            Texture2D levelTexture = _levelTexturesDatabase.GetLevelTexture(levelNumber);

            if (levelTexture == null)
            {
                Debug.LogError($"Level texture not found for level {levelNumber}");
                return default;
            }

            Color[] pixels = levelTexture.GetPixels();
            int originalWidth = levelTexture.width;
            int originalHeight = levelTexture.height;

            int newWidth = originalWidth + 2;
            int newHeight = originalHeight;

            BlockType[] blockTypes = new BlockType[newWidth * newHeight];

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    int newIndex = y * newWidth + x;
                    
                    if (x == 0 || x == newWidth - 1)
                    {
                        blockTypes[newIndex] = BlockType.None;
                    }
                    else
                    {
                        int originalX = x - 1;
                        int originalPixelIndex = y * originalWidth + originalX;
                        blockTypes[newIndex] = _levelTexturesDatabase.GetBlockTypeByColor(pixels[originalPixelIndex]);
                    }
                }
            }

            return new LevelModel(new Vector2Int(newWidth, newHeight), blockTypes);
        }
    }
}