using Game.Handlers;
using Game.Player;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyMoveHandler : MoveHandler, IFixedTickable
    {
        private readonly EnemySettings _settings;
        private readonly IPlayerPositionGetter _playerPosition;

        private float _randomSpeed = 0;
        private bool _moveToPlayer = false;

        public EnemyMoveHandler(Rigidbody2D body,
            EnemySettings stats,
            PlayerFacade.Pool playerPosition) : base(body, stats)
        {
            _settings = stats;
            _playerPosition = playerPosition;
        }

        public void FixedTick()
        {
            if (!_moveToPlayer) return;

            var direction = (_playerPosition.Position - (Vector2)_body.transform.position).normalized;
            Move(direction);
        }

        public virtual void Initialize()
        {
            _randomSpeed = Random.Range(_settings.MinimalSpeed, _settings.Speed);
        }

        public void MoveToPlayer()
        {
            _moveToPlayer = true;
        }

        public void StopMoveToPlayer()
        {
            _moveToPlayer = false;
        }

        [Serializable]
        public class EnemySettings : Settings
        {
            [field: SerializeField] public float MinimalSpeed { get; private set; }
        }
    }
}