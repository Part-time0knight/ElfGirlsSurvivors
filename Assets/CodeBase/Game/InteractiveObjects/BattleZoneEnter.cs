using Core.Infrastructure.GameFsm;
using Game.Domain.Dto;
using Game.Infrastructure.States;
using Game.Units;
using System;
using UnityEngine;
using Zenject;

namespace Game.InteractiveObjects
{
    public class BattleZoneEnter : UnitFacade
    {
        private IGameStateMachine _gameFsm;
        private Vector2 _position;

        public override void MakeCollision(DamageDto damageDto)
        {
            _gameFsm.Enter<Battle>();
        }

        [Inject]
        private void Construct(IGameStateMachine stateMachine,
            ZoneEnterSettings zoneEnter)
        {
            _gameFsm = stateMachine;
            _position = zoneEnter.SpawnPosition;
        }

        private void OnSpawn()
        {
            transform.position = _position;
        }

        public class Pool : MonoMemoryPool<BattleZoneEnter>
        {
            protected override void Reinitialize(BattleZoneEnter item)
            {
                base.Reinitialize(item);
                item.OnSpawn();
            }
        }

        [Serializable]
        public class ZoneEnterSettings : Settings
        {
            [field: SerializeField] public Vector2 SpawnPosition { get; private set; }
        }
    }
}