using Game.Units;
using System;
using UnityEngine;
using Zenject;

namespace Game.Enemies
{
    public class BatFacade : EnemyFacade
    {
        [Inject]
        private void Construct(BatSettings settings,
            EnemyDamageHandler damageHandler,
            EnemyMoveHandler moveHandler,
            EnemyWeaponHandler weaponHandler,
            SignalBus signalBus)
        {
            base.Construct(settings, damageHandler, moveHandler, weaponHandler, signalBus);
        }

        [Serializable]
        public class BatSettings : EnemySettings
        {

        }
    }
}