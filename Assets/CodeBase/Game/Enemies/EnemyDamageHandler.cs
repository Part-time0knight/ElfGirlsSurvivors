using Game.Domain.Dto;
using Game.Handlers;
using Game.Infrastructure;
using Game.Services;
using System;
using UnityEngine;
using Zenject;

namespace Game.Enemies
{
    public class EnemyDamageHandler : DamageHandler
    {
        public event Action<int> HitUpdate;

        private readonly SignalBus _signalBus;

        public EnemyDamageHandler(EnemySettings stats,
            ModifiersService modifiersService,
            SignalBus signalBus) 
            : base(stats, modifiersService)
        {
            _signalBus = signalBus;
        }

        public override void TakeDamage(DamageDto damage)
        {
            base.TakeDamage(damage);
            HitUpdate?.Invoke(_hits);
        }

        protected override void CalculationHealth(DamageDto dto)
        {
            base.CalculationHealth(dto);
            _signalBus.Fire<Signals.PlayerMakeDamage>(new() { Damage = dto.Damage });
        }

        [Serializable]
        public class EnemySettings : Settings
        {

        }
    }
}