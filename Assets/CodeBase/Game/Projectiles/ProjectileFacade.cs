using Game.Domain.Dto;
using Game.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Projectiles
{
    public class ProjectileFacade : UnitFacade, IProjectile
    {
        public event Action<IProjectile, GameObject> OnHit;
        public event Action<IProjectile> OnDead;

        protected Vector2 _direction = Vector2.zero;
        protected ProjectileMoveHandler _bulletMove;
        protected ProjectileDamageHandler _damageHandler;

        protected readonly List<GameObject> _ignoreObjects = new();
        protected readonly List<string> _ignoreTags = new();

        public override bool Pause
        {
            set
            {
                if (value)
                    SetPause();
                else
                    Continue();
            }
        }

        public Vector2 Direction => _bulletMove.Direction;

        public Vector2 Position => transform.position;

        public void SetIgnore(GameObject unit)
            => _ignoreObjects.Add(unit);

        public void SetIgnore(string tag)
            => _ignoreTags.Add(tag);

        public void RemoveIgnore(GameObject unit)
            => _ignoreObjects.Remove(unit);

        public void RemoveIgnore(string tag)
            => _ignoreTags.Remove(tag);


        public virtual void Initialize(Vector2 startPos, Vector2 targetPos)
        {
            _ignoreObjects.Clear();
            _ignoreTags.Clear();
            transform.position = startPos;
            _direction = (targetPos - startPos).normalized;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            _damageHandler.Reset();
            _bulletMove.Move(_direction);
        }

        public override void MakeCollision(DamageDto damageDto)
        {
            if (IgnoreCheck(damageDto.DamageDealer.gameObject))
                return;
            _damageHandler.TakeDamage(damageDto);
        }

        public void AddAcceleration(Vector2 acceleration)
        {
            _bulletMove.AddAcceleration(acceleration);
        }

        protected virtual void SetPause()
        {
            _bulletMove.Pause();
        }

        protected virtual void Continue()
        {
            _bulletMove.Continue();
        }

        protected virtual void Awake()
        {
            _bulletMove.OnTrigger += InvokeHit;
            _bulletMove.OnCollision += InvokeHit;
            _damageHandler.OnDeath += InvokeDeath;
        }


        [Inject]
        protected virtual void Construct(ProjectileMoveHandler bulletMove,
            ProjectileDamageHandler projectileDamageHandler)
        {
            _bulletMove = bulletMove;
            _damageHandler = projectileDamageHandler;
        }

        protected virtual void InvokeHit(GameObject objectHit)
        {
            if (IgnoreCheck(objectHit))
                return;
            OnHit?.Invoke(this, objectHit);
        }

        protected virtual void OnDestroy()
        {
            _bulletMove.OnTrigger -= InvokeHit;
            _bulletMove.OnCollision -= InvokeHit;
            _damageHandler.OnDeath -= InvokeDeath;
        }

        protected virtual void InvokeDeath()
        {
            OnDead?.Invoke(this);
        }

        protected bool IgnoreCheck(GameObject target)
        {
            foreach (var gObject in _ignoreObjects)
                if (gObject == target)
                    return true;
            foreach (var tag in _ignoreTags)
                if (target.CompareTag(tag))
                    return true;

            return false;
        }

        public class Pool : MonoMemoryPool<Vector2, Vector2, ProjectileFacade>, IProjectilePool
        {
            public virtual void DespawnProjectile(IProjectile projectile)
            {
                var bullet = projectile as ProjectileFacade;
                if (projectile == null || bullet == null) return;

                bullet.Pause = true;
                Despawn(bullet);
                
            }

            public virtual IProjectile SpawnProjectile(Vector2 startPos, Vector2 target)
            {
                var item = Spawn(startPos, target);
                return item;
            }

            /// <param name="startPos">World space position</param>
            /// <param name="targetPos">World space position</param>
            protected override void Reinitialize(Vector2 startPos, Vector2 targetPos, ProjectileFacade item)
            {
                base.Reinitialize(startPos, targetPos, item);
                item.Initialize(startPos, targetPos);
            }


        }
    }
}