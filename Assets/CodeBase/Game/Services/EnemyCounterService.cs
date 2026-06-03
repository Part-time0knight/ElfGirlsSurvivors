using Game.Infrastructure;
using System;
using UnityEngine;
using Zenject;

namespace Game.Services
{
    public class EnemyCounterService : IInitializable, IDisposable
    {
        public event Action AllEnemySpawned;
        public event Action AllEnemyDead;

        public int Killed => _enemyDead;

        private readonly SignalBus _signalBus;
        private readonly Settings _settings;

        private int _enemySpawned;
        private int _enemyDead;

        public EnemyCounterService(SignalBus signalBus,
            Settings settings)
        {
            _signalBus = signalBus;
            _settings = settings;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<Signals.EnemySpawn>(InvokeEnemySpawned);
            _signalBus.Subscribe<Signals.EnemyDeath>(InvokeEnemyDied);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<Signals.EnemySpawn>(InvokeEnemySpawned);
            _signalBus.Unsubscribe<Signals.EnemyDeath>(InvokeEnemyDied);
        }

        public void Reset()
        {
            _enemySpawned = 0;
            _enemyDead = 0;
        }

        private void InvokeEnemySpawned()
        {
            _enemySpawned++;
            if (_enemySpawned >= _settings.EnemyCount)
                AllEnemySpawned?.Invoke();
        }

        private void InvokeEnemyDied()
        {
            _enemyDead++;
            if (_enemyDead >= _settings.EnemyCount)
                AllEnemyDead?.Invoke();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public int EnemyCount;
        }

    }
}