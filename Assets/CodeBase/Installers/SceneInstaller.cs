using Core.MVVM.Windows;
using Game.Domain.Factories;
using Game.Enemies;
using Game.Infrastructure;
using Game.InteractiveObjects;
using Game.Player;
using Game.Presentation.ViewModel;
using Game.Projectiles;
using Game.Services;
using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            InstallFactories();
            InstallServices();
            InstallFsm();
            InstallViewModel();
        }

        private void InstallViewModel()
        {
            Container
                .BindInterfacesAndSelfTo<FinishViewModel>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<HUDViewModel>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallServices()
        {
            Container
                .BindInterfacesAndSelfTo<EnemyCounterService>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<PlayerDamageService>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<ModifiersService>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<ScoreService>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<SaveService>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallFactories()
        {
            Container
                .BindInterfacesAndSelfTo<StatesFactory>()
                .AsSingle()
                .NonLazy();

            Container
                .BindMemoryPool<ProjectileFacade, ProjectileFacade.Pool>()
                .FromComponentInNewPrefab(_settings.ProjectilePrefab)
                .UnderTransform(_settings.ProjectileContainer);

            Container
                .BindMemoryPool<PlayerFacade, PlayerFacade.Pool>()
                .FromComponentInNewPrefab(_settings.PlayerPrefab);

            Container
                .BindMemoryPool<BattleZoneEnter, BattleZoneEnter.Pool>()
                .FromComponentInNewPrefab(_settings.zoneEnterPrefab);

            Container
                .BindMemoryPool<BatFacade, BatFacade.Pool>()
                .FromComponentInNewPrefab(_settings.BatPrefab)
                .UnderTransform(_settings.BatContainer);

            Container
                .BindInterfacesAndSelfTo<EnemySpawner>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallFsm()
        {
            Container
                .BindInterfacesAndSelfTo<WindowFsm>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<GameFsm>()
                .AsSingle()
                .NonLazy();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public PlayerFacade PlayerPrefab { get; private set; }

            [field: SerializeField] public ProjectileFacade ProjectilePrefab { get; private set; }
            [field: SerializeField] public Transform ProjectileContainer { get; private set; }

            [field: SerializeField] public BatFacade BatPrefab { get; private set; }
            [field: SerializeField] public Transform BatContainer { get; private set; }

            [field: SerializeField] public BattleZoneEnter zoneEnterPrefab { get; private set; }
        }
    }
}