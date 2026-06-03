using Game.Domain.Dto;
using Game.Misc;
using Game.StaticData;
using Game.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Handlers
{
    public class WeaponHandler
    {
        protected readonly Settings _settings;
        protected readonly Timer _reloadTimer = new();
        protected readonly Timer _delayTimer = new();

        protected UnitFacade _self;

        protected UnitFacade _target;


        public WeaponHandler(UnitFacade unit,
            Settings settings)
        {
            _settings = settings;
            _self = unit;
        }

        public virtual void FrameDamage(UnitFacade target)
        {
            if (_reloadTimer.Active)
                return;
            _target = target;
            DamageWithFrameDelay();
        }

        private void DamageWithFrameDelay()
        {
            if (_delayTimer.Active)
                return;
            _delayTimer.Initialize(Time.fixedDeltaTime, MakeDamage).Play();
        }

        protected virtual void MakeDamage()
        {
            HashSet<Modifier> modifiers = new();

            foreach (var mod in _settings.Modifiers)
                if (!modifiers.Contains(mod))
                    modifiers.Add(mod);

            DamageDto dto = new()
            {
                Damage = _settings.Damage,
                Modifiers = modifiers,
                DamageDealer = _self,
            };
            _target.MakeCollision(dto);
            _reloadTimer.Initialize(_settings.DamageDelay).Play();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public int Damage { get; private set; }
            [field: SerializeField] public float DamageDelay { get; private set; }

            [field: SerializeField] public List<Modifier> Modifiers { get; private set; }
        }
    }
}