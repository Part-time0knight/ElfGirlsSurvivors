using Game.Enemies;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "EnemiesSettingsInstaller", menuName = "Installers/EnemiesSettings")]
    public class EnemiesSettingsInstaller : ScriptableObjectInstaller<EnemiesSettingsInstaller>
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(_settings.BatSettings)
                .AsSingle();

            Container
                .BindInstance(_settings.MoveSettings)
                .AsSingle();

            Container
                .BindInstance(_settings.DamageSettings)
                .AsSingle();

            Container
                .BindInstance(_settings.WeaponSettings)
                .AsSingle();

            Container
                .BindInstance(_settings.SpawnerSettings)
                .AsSingle();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public BatFacade.BatSettings BatSettings { get; private set; }
            [field: SerializeField] public EnemyMoveHandler.EnemySettings MoveSettings { get; private set; }
            [field: SerializeField] public EnemyDamageHandler.EnemySettings DamageSettings { get; private set; }
            [field: SerializeField] public EnemyWeaponHandler.EnemySettings WeaponSettings { get; private set; }

            [field: SerializeField] public EnemySpawner.Settings SpawnerSettings { get; private set; }
        }
    }
}