using UnityEngine;

namespace Ballon
{
    public class BallonView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BallonMovement _movementComponent;
        public BallonMovement MovementComponent => _movementComponent;

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        public void SetSortingOrder(int order)
        {
            _spriteRenderer.sortingOrder = order;
        }
    }
}
