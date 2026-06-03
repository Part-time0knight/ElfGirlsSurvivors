using Game.Handlers;
using System;
using UnityEngine;

namespace Game.Projectiles
{
    public class ProjectileMoveHandler : MoveHandler
    {
        public Vector2 Direction => Velocity.normalized;

        public ProjectileMoveHandler(Rigidbody2D body, ProjectileSettings stats) 
            : base(body, stats)
        {
        }

        public void AddAcceleration(Vector2 acceleration)
        {
            _body.AddForce(acceleration, ForceMode2D.Force);
        }

        [Serializable]
        public class ProjectileSettings : Settings
        { }
    }
}