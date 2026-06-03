using Game.InteractiveObjects;
using Game.Services;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "BattleSettingsInstaller", menuName = "Installers/BattleSettingsInstaller")]
    public class BattleSettingsInstaller : ScriptableObjectInstaller<BattleSettingsInstaller>
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(_settings.EnterSettings)
                .AsSingle();

            Container
                .BindInstance(_settings.EnemyesCount)
                .AsSingle();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public BattleZoneEnter.ZoneEnterSettings EnterSettings { get; private set; }

            [field: SerializeField]
            public EnemyCounterService.Settings EnemyesCount { get; private set; }
        }
    }
}