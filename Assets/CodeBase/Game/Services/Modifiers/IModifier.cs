using Game.Domain.Dto;
using UnityEngine;

namespace Game.Services.Modifiers
{
    public interface IModifier
    {
        void SetModifier(DamageDto damageDto);
    }
}