using UnityEngine;

namespace Ballon
{
    public class BallonMovement : MonoBehaviour
    {
        [SerializeField] private float _minFlySpeed = 1.0f;
        [SerializeField] private float _maxFlySpeed = 3.0f;
        [SerializeField] private float _minFrequency = 0.5f;
        [SerializeField] private float _maxFrequency = 2.0f;
        [SerializeField] private float _minAmplitude = 0.1f;
        [SerializeField] private float _maxAmplitude = 0.5f;
        
        private Vector2 _direction;        
        private float _timeOffset;
        
        private float _speed;               
        private float _frequency;            
        private float _amplitude;           

        public void Initialize(Vector2 direction)
        {
            _direction = direction;
            
            _speed = Random.Range(_minFlySpeed, _maxFlySpeed);
            _frequency = Random.Range(_minFrequency, _maxFrequency);
            _amplitude = Random.Range(_minAmplitude, _maxAmplitude);
            
            _timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 linearMove = (Vector3)_direction * (_speed * Time.deltaTime);
            
            float time = Time.time + _timeOffset;
            
            Vector2 perpendicularDirection = new Vector2(-_direction.y, _direction.x);
            float sinValue = Mathf.Sin(time * _frequency);
            Vector3 sinusoidalMove = (Vector3)perpendicularDirection * (sinValue * _amplitude);
            
            transform.position += linearMove + sinusoidalMove * Time.deltaTime;
        }
    }
}
