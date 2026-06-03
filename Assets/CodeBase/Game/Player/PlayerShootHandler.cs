using Cysharp.Threading.Tasks;
using Game.Handlers;
using Game.Infrastructure;
using Game.Projectiles;
using Game.StaticData;
using Game.Units;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerShootHandler : ShootHandler
    {
        private Func<Vector2> _targetGetter;
        private Func<Vector2> _positionGetter;

        private CancellationTokenSource _cts = null;

        public bool Active
        {
            set
            {
                if (value)
                    StartAutomatic();
                else
                    StopAutomatic();
            }
        }

        public bool IsPause
        {
            set
            {
                if (value)
                    Pause();
                else
                    Continue();
            }
        }

        public PlayerShootHandler(ProjectileFacade.Pool projectilePool,
            PlayerSettings settings,
            PlayerFacade playerFacade) : base(
                projectilePool,
                playerFacade,
                settings)
        {
        }

        public void StartAutomatic()
        {
            if (_cts != null)
                return;
            _cts = new();
            Repeater().Forget();
        }

        public void StopAutomatic()
        {
            if (_cts == null)
                return;
            _cts.Cancel();
            _cts = null;
        }

        public void Dispose()
        {
            StopAutomatic();
            Clear();
        }


        public void SetTarget(Func<Vector2> targetGetter)
        {
            _targetGetter = targetGetter;
        }

        public void SetPosition(Func<Vector2> positionGetter)
        {
            _positionGetter = positionGetter;
        }

        public override IProjectile CreateProjectile(Vector2 weaponPos, Vector2 target)
        {
            var bullet = base.CreateProjectile(weaponPos, target);
            bullet.SetIgnore(Tags.PlayerProjectile);
            return bullet;
        }

        protected override void MakeDamage(IProjectile iBullet, UnitFacade unit)
        {
            base.MakeDamage(iBullet, unit);
        }

        protected override void InvokeHit(IProjectile projectile, UnitFacade unitHandler)
        {
            base.InvokeHit(projectile, unitHandler);
            if (unitHandler == null)
                return;
        }

        private async UniTask Repeater()
        {
            Vector2 target;
            Vector2 position;
            do
            {
                await UniTask.WaitWhile(() => _timer.Active, cancellationToken: _cts.Token);
                target = _targetGetter.Invoke();
                position = _positionGetter.Invoke();
                if (!_cts.IsCancellationRequested)
                    Shoot(position, target);
            } while (!_cts.IsCancellationRequested);
        }

        [Serializable]
        public class PlayerSettings : Settings
        { }
    }
}