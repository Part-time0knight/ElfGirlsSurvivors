using System;
using UnityEngine;

namespace Game.Projectiles
{
    public interface IProjectile 
    {
        event Action<IProjectile, GameObject> OnHit;
        public event Action<IProjectile> OnDead;

        public Vector2 Direction { get; }
        public Vector2 Position { get; }

        void Initialize(Vector2 startPos, Vector2 targetPos);

        bool Pause { set; }

        void SetIgnore(GameObject unit);
        void SetIgnore(string tag);

        void RemoveIgnore(GameObject unit);
        void RemoveIgnore(string tag);

        void AddAcceleration(Vector2 acceleration);
    }
}