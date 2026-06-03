using Game.Domain.Dto;
using Game.Handlers;
using Game.Infrastructure;
using Game.StaticData;
using Game.Units;
using System;
using UnityEngine;
using Zenject;

namespace Game.Enemies
{
    public abstract class EnemyFacade : UnitFacade
    {
        public event Action<EnemyFacade> OnDeath;

        protected EnemyDamageHandler _damageHandler;
        protected EnemyMoveHandler _moveHandler;
        protected EnemyWeaponHandler _weaponHandler;
        protected SignalBus _signalBus;
        protected EnemySettings _settings;

        public override void MakeCollision(DamageDto dto)
        {
            _damageHandler.TakeDamage(dto);
        }

        protected virtual void Construct(EnemySettings settings,
            EnemyDamageHandler damageHandler,
            EnemyMoveHandler moveHandler,
            EnemyWeaponHandler weaponHandler,
            SignalBus signalBus)
        {
            _damageHandler = damageHandler;
            _moveHandler = moveHandler;
            _weaponHandler = weaponHandler;
            _settings = settings;
            _signalBus = signalBus;
        }

        private void InvokeSpawn(Vector2 position)
        {
            transform.position = position;
            _moveHandler.MoveToPlayer();
            _damageHandler.Reset();
            _damageHandler.OnDeath += InvokeDeath;
            _moveHandler.OnTrigger += InvokeTrigger;
        }

        private void InvokeDespawn()
        {
            _moveHandler.Stop();
            _moveHandler.StopMoveToPlayer();
            _damageHandler.OnDeath -= InvokeDeath;
            _moveHandler.OnTrigger -= InvokeTrigger;
        }

        private void InvokeDeath()
        {
            _signalBus.Fire<Signals.AddScore>(new() { Score = _settings.Score });
            OnDeath?.Invoke(this);
        }

        private void InvokeTrigger(GameObject gameObject)
        {
            var facade = gameObject.GetComponent<UnitFacade>();
            if (!facade || gameObject.CompareTag(Tags.Enemy)) return;
            _weaponHandler.FrameDamage(facade);
        }

        [Serializable]
        public class EnemySettings : Settings
        {
            [field: SerializeField] public int Score {  get; private set; }
        }

        public class Pool : MonoMemoryPool<Vector2, EnemyFacade>
        {
            protected override void OnDespawned(EnemyFacade item)
            {
                base.OnDespawned(item);
                item.InvokeDespawn();
            }

            protected override void Reinitialize(Vector2 spawnPoint,
                EnemyFacade item)
            {
                item.InvokeSpawn(spawnPoint);
                base.Reinitialize(spawnPoint, item);
                
            }
        }
    }
}