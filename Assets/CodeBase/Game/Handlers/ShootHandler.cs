using Game.Domain.Dto;
using Game.Misc;
using Game.Projectiles;
using Game.StaticData;
using Game.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Handlers
{
    public class ShootHandler : IInitializable
    {

        public event Action<IProjectile, UnitFacade> OnHit;

        protected readonly Settings _settings;
        protected readonly Timer _timer = new();
        protected readonly Timer _damageDelay = new();

        protected readonly List<IProjectile> _bullets = new();
        protected readonly IProjectilePool _bulletPools;

        protected readonly UnitFacade _unitFacade;
        protected UnitFacade _targetFacade;

        public ShootHandler(ProjectileFacade.Pool projectilePool,
            UnitFacade unitFacade,
            Settings settings)
        {
            _bulletPools = projectilePool;
            _settings = settings;
            _unitFacade = unitFacade;
        }


        public virtual void Initialize()
        {
            _timer.Initialize(
                time: 0f, step: 0f, null)
                .Play();
        }

        public virtual void Pause()
        {
            PauseReload();
            foreach (var bullet in _bullets)
                bullet.Pause = true;
        }

        public virtual void Clear()
        {
            while (_bullets.Count > 0)
            {
                RemoveProjectile(_bullets[0]);
            }
        }

        public virtual void Continue()
        {
            ContinueReload();
            foreach (var bullet in _bullets)
                bullet.Pause = false;
        }

        protected virtual void PauseReload()
        {
            if (!_timer.Active)
                return;
            _timer.Pause();
        }

        protected virtual void ContinueReload()
        {
            if (!_timer.Active)
                return;
            _timer.Play();
        }



        /// <param name="weaponPos">World space position</param>
        /// <param name="target">World space position</param>
        /// <param name="onReloadEnd"></param>
        public virtual void Shoot(Vector2 weaponPos, Vector2 target)
        {
            CreateProjectile(weaponPos, target);

            if (_timer.Active)
            {
                _timer.Stop();
                Debug.LogWarning(GetType().Name + " broken timer");
            }

            _timer.Initialize(
                time: _settings.AttackDelay,
                InvokeEndReload).Play();
        }

        protected virtual void Hit(IProjectile iBullet, GameObject target)
        {
            _targetFacade = target.GetComponent<UnitFacade>();

            if (target.tag == "Border")
            {
                RemoveProjectile(iBullet);
                return;
            }

            if (_targetFacade != null)
            {
                MakeDamage(iBullet, _targetFacade);
            }
        }

        public virtual IProjectile CreateProjectile(Vector2 weaponPos, Vector2 target)
        {
            var currentBullet = _bulletPools
                .SpawnProjectile(weaponPos, target);
            currentBullet.SetIgnore(_unitFacade.gameObject);
            _bullets.Add(currentBullet);
            currentBullet.OnHit += Hit;
            currentBullet.OnDead += RemoveProjectile;
            return currentBullet;
        }

        protected virtual void MakeDamage(IProjectile iBullet, UnitFacade unit)
        {
            Timer timer = new();

            HashSet<Modifier> modifiers = new();

            foreach (var mod in _settings.Modifiers)
                if (!modifiers.Contains(mod))
                    modifiers.Add(mod);

            DamageDto damageDto = new() 
            {
                Damage = _settings.Damage,
                Modifiers = modifiers,
                DamageDealer = _unitFacade,
            };

            timer.Initialize(Time.fixedDeltaTime,
                () => {
                    unit.MakeCollision(damageDto);
                    InvokeHit(iBullet, _targetFacade);
                }).Play();
        }



        protected void RemoveProjectile(IProjectile iBullet)
        {
            iBullet.OnHit -= Hit;
            iBullet.OnDead -= RemoveProjectile;
            _bulletPools.DespawnProjectile(iBullet);
            _bullets.Remove(iBullet);
        }

        protected virtual void InvokeEndReload()
        { }

        protected virtual void InvokeHit(IProjectile projectile, UnitFacade unitHandler)
            => OnHit?.Invoke(projectile, unitHandler);

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public float AttackDelay { get; private set; }
            [field: SerializeField] public int Damage { get; private set; }

            [field: SerializeField] public List<Modifier> Modifiers { get; private set; }
        }
    }
}