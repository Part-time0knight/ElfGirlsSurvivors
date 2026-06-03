using Game.Domain.Dto;
using Game.Infrastructure;
using Game.Services;
using System;
using UnityEngine;
using Zenject;

namespace Game.Handlers
{
    public class DamageHandler
    {
        public virtual event Action<int> OnTakeDamage;
        public virtual event Action OnDeath;

        protected int _hits;

        protected readonly Settings _stats;
        protected readonly ModifiersService _modifiers;

        public DamageHandler(Settings stats,
            ModifiersService modifiers)
        {
            _stats = stats;
            _hits = _stats.HitPoints;
            _modifiers = modifiers;
        }


        public virtual void TakeDamage(DamageDto dto)
        {
            CalculationHealth(dto);
            if (_hits <= 0)
                OnDeath?.Invoke();
            else
                OnTakeDamage?.Invoke(_hits);
        }

        public virtual void Reset()
        {
            _hits = _stats.HitPoints;
        }

        protected virtual void CalculationHealth(DamageDto dto)
        {
            foreach (var mod in dto.Modifiers)
                _modifiers.UseModifier(mod, dto);
            _hits -= dto.Damage;
            _hits = Mathf.Max(Mathf.Min(_hits, _stats.HitPoints), 0);
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public int HitPoints { get; protected set; }
        }
    }
}