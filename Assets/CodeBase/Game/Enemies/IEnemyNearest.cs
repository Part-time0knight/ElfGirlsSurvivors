using UnityEngine;

namespace Game.Enemies
{
    public interface IEnemyNearest
    {
        Vector2 GetNearist(Vector2 pos);
    }
}