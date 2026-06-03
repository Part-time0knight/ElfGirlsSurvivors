using Core.Infrastructure.GameFsm;
using Game.Domain.Dto;
using Game.Player.Fsm.States;
using Game.Units;
using System;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerFacade : UnitFacade
    {
        public event Action<DamageDto> OnDamage;
        public event Action OnDeath;

        private IGameStateMachine _playerFsm;
        private PlayerSettings _settings;
        
        public void EnterIdle()
        {
            _playerFsm.Enter<Idle>();
        }

        public void EnterDeactive()
        {
            _playerFsm.Enter<Deactive>();
        }

        public void EnterBattle()
        {
            _playerFsm.Enter<Battle>();
        }

        public override void MakeCollision(DamageDto dto)
        {
            OnDamage?.Invoke(dto);
        }

        public void CallDeath()
        {
            OnDeath?.Invoke();
        }

        [Inject]
        private void Construct(IGameStateMachine playerFsm,
            PlayerSettings settings)
        {
            _playerFsm = playerFsm;
            _settings = settings;
        }

        private void InvokeCreate()
        {
            _playerFsm.Enter<Initialize>();
        }

        private void OnSpawn()
        {
            transform.position = _settings.SpawnPoint;
        }

        [Serializable]
        public class PlayerSettings : Settings
        {
            [field: SerializeField] public float WeaponStartRadius { get; private set; }
            [field: SerializeField] public Vector2 SpawnPoint { get; private set; }
        }

        public class Pool : MonoMemoryPool<PlayerFacade>, IPlayerPositionGetter
        {
            public Vector2 Position => _lastFacade.GetPosition();

            private PlayerFacade _lastFacade;

            protected override void OnCreated(PlayerFacade item)
            {
                base.OnCreated(item);
                item.InvokeCreate();
            }

            protected override void Reinitialize(PlayerFacade item)
            {
                base.Reinitialize(item);
                item.OnSpawn();
                _lastFacade = item;
            }
        }
    }

    public interface IPlayerPositionGetter
    {
        Vector2 Position { get; }
    }
}