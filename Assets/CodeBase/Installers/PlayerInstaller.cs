using Game.Domain.Factories;
using Game.Player;
using Game.Player.Fsm;
using Game.Projectiles;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            InstallFactories();
            InstallPlayerComponents();
            InstallFsm();
        }

        private void InstallFactories()
        {
            Container
                .BindInterfacesAndSelfTo<StatesFactory>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallFsm()
        {
            Container
                .BindInterfacesAndSelfTo<PlayerFsm>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallPlayerComponents()
        {
            Container
                .BindInstance(_settings.Body)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerMoveHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerInputHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerShootHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerDamageHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerCollision>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerDash>()
                .AsSingle();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public Rigidbody2D Body { get; private set; }
        }
    }
}