using System.Threading;
using Block.Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Block
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AnimatorComponent _animator;
        
        public BlockType Type { get; private set; }
        public Vector2Int GridPosition { get;set; }
        
        public void Initialize(BlockType blockType, AnimatorOverrideController animatorController)
        {
            Type = blockType;
            _animator.SetAnimatorController(animatorController);
            _animator.PlayIdleAnimation(Random.Range(0f,0.5f));
        }
        
        public void UpdateGridPosition(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public UniTask MoveTo(Vector2 targetPosition, float duration, bool isDown, CancellationToken cancellationToken)
        {
            Ease easeType = isDown ? Ease.InQuad : Ease.OutQuad;
             return transform.DOMove(targetPosition, duration)
                 .SetEase(easeType)
                 .ToUniTask(cancellationToken: cancellationToken);
        }

        public UniTask AnimateDestruction(CancellationToken cancellationToken)
        {
            return _animator.PlayDestroyAnimation(cancellationToken);
        }

        public void SetSize(Vector2 size)
        {
            if (_spriteRenderer.sprite == null) return;

            Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;
            Vector2 scale = size / spriteSize;
            _spriteRenderer.transform.localScale = new Vector3(scale.x, scale.y, 1);
        }
    }
}