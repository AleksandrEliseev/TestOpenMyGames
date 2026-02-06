using System.Collections.Generic;
using Ballon.Settings;
using GameCamera;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ballon
{
    public class BallonSpawner : IStartable, ITickable
    {
        private readonly Transform _container;
        private readonly BallonConfig _ballonConfig;
        private readonly CameraContainer _cameraContainer;
        
        private readonly List<BallonView> _activeBallons = new List<BallonView>();
        private const int MaxBallons = 3;
        private const int PoolSize = 4;
        
        private readonly Queue<BallonView> _ballonPool = new Queue<BallonView>();

        private Camera _camera;
        
        private float _leftBound;
        private float _rightBound;
        private float _bottomBound;
        private float _topBound;

        private float _spawnTimer;              
        private float _currentSpawnDelay;         

        [Inject]
        public BallonSpawner(BallonConfig ballonConfig, CameraContainer cameraContainer, Transform container, LifetimeScope lifetimeScope)
        {
            _ballonConfig = ballonConfig;
            _cameraContainer = cameraContainer;
            _container = container;
        }

        public void Start()
        {
            _camera = _cameraContainer.GameCamera;
            CacheCameraBounds();
            
            for (int i = 0; i < PoolSize; i++)
            {
                var ballon = Object.Instantiate(_ballonConfig.BallonPrefab, _container);
                ballon.gameObject.SetActive(false);
                _ballonPool.Enqueue(ballon);
            }
        }
        
        public void Tick()
        {
            if (_camera == null)
                return;
            
            for (int i = _activeBallons.Count - 1; i >= 0; i--)
            {
                var ballon = _activeBallons[i];
                if (ballon == null)
                {
                    _activeBallons.RemoveAt(i);
                    continue;
                }

                if (IsBallonOffScreen(ballon))
                {
                    DespawnBallon(ballon);
                }
            }
            
            if (_activeBallons.Count < MaxBallons && _ballonPool.Count > 0)
            {
                _spawnTimer -= Time.deltaTime;
                if (_spawnTimer <= 0f)
                {
                    SpawnBallon();
                    ResetSpawnTimer();
                }
            }
        }

        private void ResetSpawnTimer()
        {
            _currentSpawnDelay = Random.Range(_ballonConfig.MinDelayBetweenSpawns, _ballonConfig.MaxDelayBetweenSpawns);
            _spawnTimer = _currentSpawnDelay;
        }

        private void CacheCameraBounds()
        {
            if (_camera == null)
                return;

            var camPos = _camera.transform.position;
            float height = 2f * _camera.orthographicSize;
            float width = height * _camera.aspect;

            _leftBound = camPos.x - width * 0.5f;
            _rightBound = camPos.x + width * 0.5f;
            _bottomBound = camPos.y - height * 0.3f;
            _topBound = camPos.y + height * 0.3f;
        }

        private bool IsBallonOffScreen(BallonView ballon)
        {
            Vector3 pos = ballon.transform.position;

            const float margin = 2.0f;

            return pos.x < _leftBound - margin ||
                   pos.x > _rightBound + margin ||
                   pos.y < _bottomBound - margin *2 ||
                   pos.y > _topBound + margin * 2;
        }

        private void SpawnBallon()
        {
            if (_ballonPool.Count == 0)
                return;

            var ballon = _ballonPool.Dequeue();
            ballon.gameObject.SetActive(true);

            SetupBallon(ballon);
            _activeBallons.Add(ballon);
        }

        private void SetupBallon(BallonView ballon)
        {
            Vector2 spawnPos = GetRandomSpawnPosition();
            ballon.transform.position = spawnPos;

            Vector2 direction = GetRandomDirection(spawnPos);

            ballon.MovementComponent.Initialize(direction);

            var sprite = _ballonConfig.GetRandomSprite();
            ballon.SetSprite(sprite);
        }

        private void DespawnBallon(BallonView ballon)
        {
            _activeBallons.Remove(ballon);
            ballon.gameObject.SetActive(false);
            _ballonPool.Enqueue(ballon);
        }

        private Vector2 GetRandomSpawnPosition()
        {
            float spawnY = Random.Range(_bottomBound, _topBound);

            bool spawnFromLeft = Random.value < 0.5f;
            const float xOffset = 1.0f;

            float spawnX = spawnFromLeft ? _leftBound - xOffset : _rightBound + xOffset;

            return new Vector2(spawnX, spawnY);
        }

        private Vector2 GetRandomDirection(Vector2 spawnPosition)
        {
            bool fromLeft = spawnPosition.x <= (_leftBound + _rightBound) * 0.5f;

            float horizontal = fromLeft
                ? Random.Range(0.8f, 1.0f)   // right
                : Random.Range(-1.0f, -0.8f); // left
            
            float vertical = Random.Range(-0.25f, 0.25f);

            var dir = new Vector2(horizontal, vertical);
            return dir.normalized;
        }
    }
}
