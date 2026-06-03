using UnityEngine;

namespace Game.Projectiles
{
    public interface IProjectilePool
    {
        /// <param name="start">World space position</param>
        /// <param name="target">World space position</param>
        IProjectile SpawnProjectile(Vector2 start, Vector2 target);

        void DespawnProjectile(IProjectile projectile);
    }
}