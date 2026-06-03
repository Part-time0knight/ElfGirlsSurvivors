using Game.Projectiles;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "ProjectileSettingsInstaller", menuName = "Installers/ProjectileSettings")]
    public class ProjectileSettingsInstaller : ScriptableObjectInstaller<ProjectileSettingsInstaller>
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(_settings.ProjectileMove)
                .AsSingle();

            Container
                .BindInstance(_settings.ProjectileHits)
                .AsSingle();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public ProjectileMoveHandler.ProjectileSettings ProjectileMove { get; private set; }

            [field: SerializeField]
            public ProjectileDamageHandler.ProjectileSettings ProjectileHits { get; private set; }
        }
    }
}