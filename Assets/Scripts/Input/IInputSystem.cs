using System;
using Block;

namespace Input
{
    public interface IInputSystem
    {
        event Action<BlockView, Direction> OnSwipe;
        bool IsInputEnabled { get; set; }
    }
}

