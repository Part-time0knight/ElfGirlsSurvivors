using Core.Infrastructure.GameFsm;
using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;
using Game.Enemies;
using Game.Player;
using Game.Presentation.View;
using Game.Services;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class Battle : IState
    {
        private readonly PlayerFacade.Pool _playerPool;
        private readonly EnemySpawner _enemySpawner;
        private readonly EnemyCounterService _enemyCounter;
        private readonly PlayerDamageService _playerDamageService;
        private readonly IGameStateMachine _fsm;
        private readonly IWindowFsm _windowFsm;

        private PlayerFacade _player;

        public Battle(PlayerFacade.Pool playerPool,
            EnemySpawner enemySpawner,
            EnemyCounterService enemyCounterService,
            PlayerDamageService playerDamageService,
            IGameStateMachine fsm,
            IWindowFsm windowFsm)
        {
            _playerPool = playerPool;
            _enemySpawner = enemySpawner;
            _enemyCounter = enemyCounterService;
            _playerDamageService = playerDamageService;
            _fsm = fsm;
            _windowFsm = windowFsm;
        }

        public void OnEnter()
        {
            _playerDamageService.Reset();
            _enemyCounter.Reset();
            _enemySpawner.Dispose();
            _player = _playerPool.Spawn();
            _player.EnterBattle();
            _enemySpawner.Start();
            _enemyCounter.AllEnemySpawned += InvokeAllEnemySpawned;
            _enemyCounter.AllEnemyDead += InvokeAllEnemyDead;

            _player.OnDeath += InvokePlayerDead;

            _windowFsm.OpenWindow(typeof(HUDView));
        }

        public void OnExit()
        {
            _player.EnterDeactive();
            _playerPool.Despawn(_player);
            _enemyCounter.AllEnemySpawned -= InvokeAllEnemySpawned;
            _enemyCounter.AllEnemyDead -= InvokeAllEnemyDead;
            _player.OnDeath -= InvokePlayerDead;
            _enemySpawner.Reset();
            _enemySpawner.Stop();
            _windowFsm.CloseWindow(typeof(HUDView));
        }

        private void InvokeAllEnemySpawned()
        {
            _enemySpawner.Stop();
        }

        private void InvokeAllEnemyDead()
        {
            _fsm.Enter<Finish>();
        }

        private void InvokePlayerDead()
        {
            _fsm.Enter<Finish>();
        }
    }
}