using System;
using System.Collections.Generic;
using Block;
using UnityEngine;

namespace GameBoard.Level.Settings
{
    [CreateAssetMenu(fileName = "LevelTexturesDatabase", menuName = "Config/Level Textures Database")]
    public class LevelTexturesDatabase : ScriptableObject
    {
        [SerializeField] private Texture2D[] _levelTextures;
        [SerializeField] private List<LevelTypeToColor> _levelTypeToColors;

        private const float ColorTolerance = 0.1f;

        public int LevelCount => _levelTextures != null ? _levelTextures.Length : 0;
        
        public Texture2D GetLevelTexture(int levelIndex)
        {
            if (_levelTextures == null || _levelTextures.Length == 0)
                return null;

            if (levelIndex < 0 || levelIndex >= _levelTextures.Length)
                return null;

            return _levelTextures[levelIndex];
        }
        
        public BlockType GetBlockTypeByColor(Color color)
        {
            foreach (var mapping in _levelTypeToColors)
            {
                if (IsColorsEquals(color, mapping.Color))
                {
                    return mapping.BlockType;
                }
            }

            return BlockType.None;
        }

        private bool IsColorsEquals(Color a, Color b)
        {
            // расстояние в RGB
            float dr = a.r - b.r;
            float dg = a.g - b.g;
            float db = a.b - b.b;
            float distanceSq = dr * dr + dg * dg + db * db;

            // квадрат допуска (чтобы не делать Sqrt)
            float toleranceSq = ColorTolerance * ColorTolerance;

            if (distanceSq > toleranceSq)
                return false;

            return true;
        }

        [Serializable]
        public struct LevelTypeToColor
        {
            public BlockType BlockType;
            public Color Color;
        }
    }
}