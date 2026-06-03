using Cysharp.Threading.Tasks;
using Game.Infrastructure;
using Game.Player;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Timer = Game.Misc.Timer;

namespace Game.Enemies
{
    public class EnemySpawner : IDisposable, IEnemyNearest
    {
        private readonly Timer _timer;

        private readonly EnemyFacade.Pool _pool;
        private readonly EnemySpawner.Settings _settings;
        private readonly IPlayerPositionGetter _playerPosition;
        private readonly SignalBus _signalBus;

        private readonly List<EnemyFacade> _enemies;

        private Vector2 _position;
        private Vector2 _direction;

        private CancellationTokenSource _cts = null;


        public EnemySpawner(EnemyFacade.Pool pool,
            EnemySpawner.Settings settings,
            PlayerFacade.Pool playerPosition,
            SignalBus signalBus)
        {
            _timer = new();
            _enemies = new();
            _pool = pool;
            _settings = settings;
            _playerPosition = playerPosition;
            _signalBus = signalBus;
        }

        public Vector2 GetNearist(Vector2 pos)
        {
            Vector2 result = new Vector2(100f, 0f);
            foreach (var enemy in _enemies)
            {
                if (Vector2.Distance(pos, enemy.GetPosition()) <
                    Vector2.Distance(result, pos))
                    result = enemy.GetPosition();
            }

            return result;
        }

        public void Start()
        {
            if (_cts != null)
                return;
            
            _cts = new();
            _timer.Initialize(_settings.Delay).Play();
            Repeater().Forget();
        }

        public void Stop()
        {
            if (_cts == null)
                return;
            _cts.Cancel();
            
        }

        public void Reset()
        {
            while (_enemies.Count > 0)
                InvokeDeath(_enemies[0]);
        }

        public void Pause()
        {
            if (_timer.Active)
                _timer.Pause();
            foreach (var enemy in _enemies)
                enemy.Pause = true;
        }

        public void Continue()
        {
            if (_timer.Active)
                _timer.Play();
            foreach (var enemy in _enemies)
                enemy.Pause = false;
        }

        public void Dispose()
        {
            Stop();
            Reset();
            _cts = null;
        }

        public List<Vector2> GetPositions()
        {
            List<Vector2> positions = new();
            foreach (var enemy in _enemies)
                positions.Add(enemy.transform.position);
            return positions;
        }

        private async UniTask Repeater()
        {
            do
            {
                await UniTask.WaitWhile(() => _timer.Active,
                    cancellationToken: _cts.Token);
                _timer.Initialize(_settings.Delay).Play();
                SpawnOnBorder();
            } while (_cts != null && !_cts.IsCancellationRequested);
        }

        private void SpawnOnBorder()
        {
            CalculatePosition();
            Spawn(_position);
        }

        private void Spawn(Vector2 position)
        {
            var enemy = _pool.Spawn(position);
            enemy.OnDeath += InvokeDeath;
            _enemies.Add(enemy);
            _signalBus.Fire<Signals.EnemySpawn>();
        }

        private void InvokeDeath(EnemyFacade enemy)
        {
            _enemies.Remove(enemy);
            _pool.Despawn(enemy);
            enemy.OnDeath -= InvokeDeath;
            _signalBus.Fire<Signals.EnemyDeath>();
        }

        private void InvokeDeactivate(EnemyFacade enemyHandler)
        {
            _enemies.Remove(enemyHandler);
            _pool.Despawn(enemyHandler);
            enemyHandler.OnDeath -= InvokeDeath;
        }

        private void CalculatePosition()
        {
            _position = Vector2.zero;
            Vector2 position;

            do
            {
                bool isHorizontal = Random.Range(0, 2) == 0 ? true : false;
                bool isTop = Random.Range(0, 2) == 0 ? true : false;
                float posX, posY;
                if (isHorizontal)
                {
                    posX = Random.Range(_settings.HorizontalBorders.x, _settings.HorizontalBorders.y);
                    if (isTop)
                    {
                        posY = _settings.VerticalBorders.y;
                    }
                    else
                    {
                        posY = _settings.VerticalBorders.x;
                    }
                }
                else
                {
                    posY = Random.Range(_settings.VerticalBorders.x, _settings.VerticalBorders.y);
                    if (isTop)
                    {
                        posX = _settings.HorizontalBorders.x;
                    }
                    else
                    {
                        posX = _settings.HorizontalBorders.y;
                    }
                }
                position = new(posX, posY);
            } while (
                Vector2.Distance(position, _playerPosition.Position) <
                _settings.MinimalRangeToPlayer
                );
            _position = position;
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public Vector2 HorizontalBorders { get; private set; }
            [field: SerializeField] public Vector2 VerticalBorders { get; private set; }
            [field: SerializeField] public float MinimalRangeToPlayer { get; private set; }
            [field: SerializeField] public float Delay { get; private set; }
        }
    }
}