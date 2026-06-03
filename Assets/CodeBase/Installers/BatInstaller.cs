using Game.Enemies;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BatInstaller : MonoInstaller
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(_settings.Body)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<EnemyDamageHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<EnemyMoveHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<EnemyWeaponHandler>()
                .AsSingle();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public Rigidbody2D Body { get; private set; }
        }
    }
}