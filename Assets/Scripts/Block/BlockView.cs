using UnityEngine;

namespace Block
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BlockType _blockType;

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
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