using System;
using UnityEngine;

namespace Game.Handlers
{
    public interface IInputHandler
    {
        event Action OnMoveButtonsDown;

        event Action OnMoveButtonsUp;

        event Action<Vector2> OnMove;

        event Action OnDash;
    }
}