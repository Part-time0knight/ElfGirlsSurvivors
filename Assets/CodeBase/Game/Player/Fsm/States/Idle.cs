using Core.Infrastructure.GameFsm.States;
using Game.Enemies;
using Game.Handlers;
using Game.InteractiveObjects;
using Game.StaticData;
using Game.Units;
using UnityEngine;

namespace Game.Player.Fsm.States
{
    public class Idle : IState
    {
        private readonly IInputHandler _inputHandler;
        private readonly PlayerCollision _playerCollision;
        private readonly PlayerMoveHandler _moveHandler;
        private readonly PlayerDash _playerDash;
        private readonly PlayerDamageHandler _playerDamageHandler;

        public Idle(IInputHandler inputHandler,
            PlayerMoveHandler moveHandler,
            PlayerCollision playerCollision,
            PlayerDash playerDash,
            PlayerDamageHandler playerDamageHandler)
        {
            _inputHandler = inputHandler;
            _moveHandler = moveHandler;
            _playerCollision = playerCollision;
            _playerDash = playerDash;
            _playerDamageHandler = playerDamageHandler;
        }

        public void OnEnter()
        {
            _inputHandler.OnMove += InvokeMove;
            _inputHandler.OnDash += InvokeDash;

            _moveHandler.OnTrigger += InvokeCollision;

            _playerDamageHandler.Reset();
        }

        public void OnExit()
        {
            _inputHandler.OnMove -= InvokeMove;
            _inputHandler.OnDash -= InvokeDash;

            _moveHandler.OnTrigger -= InvokeCollision;
        }

        private void InvokeMove(Vector2 speed)
            => _moveHandler.Move(speed);

        private void InvokeCollision(GameObject collisionObject)
        {
            var facade = collisionObject.GetComponent<UnitFacade>();
            
            if (!facade) return;

            _playerCollision.MakeCollision(facade);
        }

        private void InvokeDash()
            => _playerDash.Dash();
    }
}