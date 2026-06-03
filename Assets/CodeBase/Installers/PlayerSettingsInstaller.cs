using Game.Player;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "PlayerSettingsInstaller", menuName = "Installers/PlayerSettings")]
    public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            Container.BindInstance(_settings.MoveSettings).AsSingle();
            Container.BindInstance(_settings.ShootSettings).AsSingle();
            Container.BindInstance(_settings.PlayerSettings).AsSingle();
            Container.BindInstance(_settings.DamageSettings).AsSingle();
            Container.BindInstance(_settings.DashSettings).AsSingle();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public PlayerFacade.PlayerSettings PlayerSettings { get; private set; }
            [field: SerializeField] public PlayerMoveHandler.PlayerSettings MoveSettings { get; private set; }
            [field: SerializeField] public PlayerShootHandler.PlayerSettings ShootSettings { get; private set; }
            [field: SerializeField] public PlayerDamageHandler.PlayerSettings DamageSettings { get; private set; }
            [field: SerializeField] public PlayerDash.Settings DashSettings { get; private set; }
        }
    }
}