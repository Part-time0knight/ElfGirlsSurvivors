using Game.Handlers;
using System;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerInputHandler : ITickable, IFixedTickable, IInputHandler
    {
        public event Action OnMoveButtonsDown;

        public event Action OnMoveButtonsUp;

        public event Action<Vector2> OnMove;

        public event Action OnDash;

        private bool _isHorizontal;
        private bool _isVertical;

        public bool IsMoveButtonPress
        {
            get => Input.GetButton("Vertical")
                || Input.GetButton("Horizontal");
        }

        public PlayerInputHandler()
        {
        }

        public void Tick()
        {
            if (IsMoveButtonDown())
                OnMoveButtonsDown?.Invoke();
            if (IsMoveButtonUp())
                OnMoveButtonsUp?.Invoke();


            if (IsDash())
                OnDash?.Invoke();
        }

        public void FixedTick()
        {
            _isHorizontal = MoveHorizontal() != 0;
            _isVertical = MoveVertical() != 0;

            if (_isHorizontal || _isVertical)
                OnMove?.Invoke(new(MoveHorizontal(), MoveVertical()));
        }

        private bool IsDash()
            => Input.GetButtonDown("Jump");

        private bool IsMoveButtonDown()
            => Input.GetButtonDown("Vertical")
                || Input.GetButtonDown("Horizontal");

        private bool IsMoveButtonUp()
            => (Input.GetButtonUp("Vertical")
                || Input.GetButtonUp("Horizontal"))
                && !IsMoveButtonPress;

        private float MoveVertical()
            => Input.GetAxis("Vertical");

        private float MoveHorizontal()
            => Input.GetAxis("Horizontal");
    }
}