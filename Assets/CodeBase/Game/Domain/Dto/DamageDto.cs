using Game.StaticData;
using Game.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Domain.Dto
{
    public class DamageDto
    {
        public int Damage = 0;
        public HashSet<Modifier> Modifiers = new();
        public UnitFacade DamageDealer;
    }
}