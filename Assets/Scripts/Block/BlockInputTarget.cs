using UnityEngine;

namespace Block
{
    public class BlockInputTarget : MonoBehaviour
    {
        [SerializeField] private BlockView _blockView;

        public BlockView BlockView => _blockView;
    }
}

