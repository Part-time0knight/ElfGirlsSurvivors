using Game.Domain.Dto;
using Game.Handlers;
using Game.Infrastructure;
using Game.Services;
using System;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerDamageHandler : DamageHandler
    {
        private readonly SignalBus _signalBus;

        public PlayerDamageHandler(PlayerSettings stats,
            ModifiersService modifiersService,
            SignalBus signalBus) : base(stats, modifiersService)
        {
            _signalBus = signalBus;
        }

        public override void Reset()
        {
            base.Reset();
            _signalBus.Fire<Signals.PlayerHealth>(new() { Health = _hits });
        }

        protected override void CalculationHealth(DamageDto damage)
        {
            base.CalculationHealth(damage);
            _signalBus.Fire<Signals.PlayerHealth>(new() { Health = _hits });
        }

        [Serializable]
        public class PlayerSettings : Settings
        {

        }
    }
}