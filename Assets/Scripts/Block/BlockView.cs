using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Block
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BlockType _blockType;

        public BlockType Type => _blockType;

        public Vector2Int GridPosition { get; private set; }

        public void Init(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
        
        public void UpdateGridPosition(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public UniTask MoveTo(Vector2 targetPosition, float duration,bool isDown, CancellationToken cancellationToken)
        {
            Ease easeType = isDown ? Ease.InQuad : Ease.OutQuad;
             return transform.DOMove(targetPosition, duration)
                 .SetEase(easeType)
                 .ToUniTask(cancellationToken: cancellationToken);
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