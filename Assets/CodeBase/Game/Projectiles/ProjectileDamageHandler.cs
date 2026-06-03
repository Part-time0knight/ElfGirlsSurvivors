using Game.Handlers;
using Game.Services;
using System;
using UnityEngine;

namespace Game.Projectiles
{
    public class ProjectileDamageHandler : DamageHandler
    {
        public ProjectileDamageHandler(ProjectileSettings stats, 
            ModifiersService modifiersService)
            : base(stats, modifiersService)
        { }

        [Serializable]
        public class ProjectileSettings : Settings
        { }
    }
}