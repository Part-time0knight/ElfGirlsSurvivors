using Game.Domain.Dto;
using Game.Services.Modifiers;
using Game.StaticData;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Services
{
    public class ModifiersService : IInitializable
    {
        private Dictionary<Modifier, IModifier> _modifiers = new();

        public void Initialize()
        {
            Resolve();
        }

        public void UseModifier(Modifier modifier, DamageDto damage)
        {
            _modifiers[modifier].SetModifier(damage);
        }

        private void Resolve()
        {
            _modifiers.Clear();
            _modifiers.Add(Modifier.Critical, new Critical());
        }
    }
}