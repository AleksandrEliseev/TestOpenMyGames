using UnityEngine;

namespace Ballon.Settings
{
    [CreateAssetMenu(fileName = "BallonConfig", menuName = "Configs/Ballon Config", order = 0)]
    public class BallonConfig : ScriptableObject
    {
        [SerializeField] private BallonView _ballonPrefab;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _minDelayBetweenSpawns = 10f;
        [SerializeField] private float _maxDelayBetweenSpawns = 15f;
        
        public BallonView BallonPrefab => _ballonPrefab;
        public float MinDelayBetweenSpawns => _minDelayBetweenSpawns;
        public float MaxDelayBetweenSpawns => _maxDelayBetweenSpawns;
        
        public Sprite GetRandomSprite()
        {
            if (_sprites.Length == 0)
            {
                Debug.LogWarning("BallonConfig: No sprites available.");
                return null;
            }
            int randomIndex = Random.Range(0, _sprites.Length);
            return _sprites[randomIndex];
        }
    }
}
