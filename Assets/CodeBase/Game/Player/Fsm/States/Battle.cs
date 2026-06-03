using Core.Infrastructure.GameFsm.States;
using Game.Domain.Dto;
using Game.Enemies;
using Game.Handlers;
using Game.Units;
using UnityEngine;

namespace Game.Player.Fsm.States
{
    public class Battle : IState
    {
        private readonly IInputHandler _inputHandler;
        private readonly IEnemyNearest _enemyNearest;
        private readonly PlayerMoveHandler _moveHandler;
        private readonly PlayerShootHandler _shootHandler;
        private readonly PlayerDamageHandler _damageHandler;
        private readonly PlayerFacade.PlayerSettings _settings;
        private readonly PlayerFacade _facade;
        private readonly PlayerDash _playerDash;

        public Battle(IInputHandler inputHandler,
            PlayerMoveHandler moveHandler,
            PlayerShootHandler shootHandler,
            PlayerDamageHandler damageHandler,
            PlayerFacade.PlayerSettings settings,
            PlayerFacade facade,
            PlayerDash playerDash,
            IEnemyNearest enemyNearest)
        {
            _inputHandler = inputHandler;
            _moveHandler = moveHandler;
            _shootHandler = shootHandler;
            _enemyNearest = enemyNearest;
            _damageHandler = damageHandler;
            _settings = settings;
            _playerDash = playerDash;
            _facade = facade;
        }

        public void OnEnter()
        {
            _inputHandler.OnMove += InvokeMove;
            _facade.OnDamage += InvokeTakeDamage;
            _damageHandler.OnDeath += InvokeDeath;
            _inputHandler.OnDash += InvokeDash;

            _shootHandler.StartAutomatic();
            _shootHandler.SetPosition(SetPos);
            _shootHandler.SetTarget(SetTarget);
        }

        public void OnExit()
        {
            _inputHandler.OnMove -= InvokeMove;
            _facade.OnDamage -= InvokeTakeDamage;
            _damageHandler.OnDeath -= InvokeDeath;
            _inputHandler.OnDash -= InvokeDash;

            _shootHandler.StopAutomatic();
            _moveHandler.Stop();
        }

        private void InvokeMove(Vector2 speed)
            => _moveHandler.Move(speed);

        private void InvokeTakeDamage(DamageDto damage)
        {
            _damageHandler.TakeDamage(damage);
        }

        private void InvokeDeath()
        {
            _facade.CallDeath();
        }

        private void InvokeDash()
            => _playerDash.Dash();

        private Vector2 SetTarget()
        {
            return _enemyNearest.GetNearist(_facade.GetPosition());
        }

        private Vector2 SetPos()
        {
            Vector3 position = _facade.GetPosition()
                + ((SetTarget() - _facade.GetPosition()).normalized)
                * _settings.WeaponStartRadius;

            return position;
        }
    }
}