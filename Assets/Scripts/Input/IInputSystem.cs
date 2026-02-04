using System;
using Block;
using Core;

namespace Input
{
    public interface IInputSystem
    {
        event Action<BlockView, Direction> OnSwipe;
    }
}

