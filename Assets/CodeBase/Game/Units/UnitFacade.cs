using Game.Domain.Dto;
using UnityEngine;

namespace Game.Units
{
    public abstract class UnitFacade : MonoBehaviour
    {
        public virtual bool Pause { get; set; }

        public abstract void MakeCollision(DamageDto dtor);

        public Vector2 GetPosition()
            => transform.position;

        public class Settings
        { }
    }
}