using Game.Handlers;
using Game.Units;
using System;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemyWeaponHandler : WeaponHandler
    {
        public EnemyWeaponHandler(EnemyFacade unit, EnemySettings settings) : base(unit, settings)
        {
        }

        [Serializable]
        public class EnemySettings : Settings
        {

        }
    }
}