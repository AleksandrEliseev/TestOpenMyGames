using System;
using Block;
using Core;
using DefaultNamespace;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Input
{
    public class InputSystem : IInputSystem, ITickable
    {
        private Camera _camera;
        
        private Vector2 _startTouchPosition;
        private BlockView _selectedBlock;
        private bool _isDragging;

        public event Action<BlockView, Direction> OnSwipe;

        [Inject]
        public void Construct(CameraContainer cameraContainer)
        {
            _camera = cameraContainer.GameCamera;
        }

        public void Tick()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchStart(touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        OnTouchEnd(touch.position);
                        break;
                }
                return;
            }
            
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                OnTouchStart(UnityEngine.Input.mousePosition);
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                OnTouchEnd(UnityEngine.Input.mousePosition);
            }
        }

        private void OnTouchStart(Vector3 screenPosition)
        {
            Vector2 worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);

            if (hit != null)
            {
                var inputTarget = hit.GetComponent<BlockInputTarget>();
                if (inputTarget != null)
                {
                    _selectedBlock = inputTarget.BlockView;
                    if (_selectedBlock != null)
                    {
                        _startTouchPosition = screenPosition;
                        _isDragging = true;
                    }
                }
            }
        }

        private void OnTouchEnd(Vector3 screenPosition)
        {
            if (_isDragging && _selectedBlock != null)
            {
                Vector2 endTouchPosition = screenPosition;
                Vector2 swipeDirection = endTouchPosition - _startTouchPosition;

                if (swipeDirection.magnitude > 50) // Minimum swipe distance threshold
                {
                    Direction direction = GetSwipeDirection(swipeDirection);
                    OnSwipe?.Invoke(_selectedBlock, direction);
                }

                _isDragging = false;
                _selectedBlock = null;
            }
        }

        private Direction GetSwipeDirection(Vector2 vector)
        {
            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
            {
                return vector.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return vector.y > 0 ? Direction.Up : Direction.Down;
            }
        }
    }
}
