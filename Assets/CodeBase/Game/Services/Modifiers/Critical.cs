using Game.Domain.Dto;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Services.Modifiers
{
    public class Critical : IModifier
    {
        public void SetModifier(DamageDto damageDto)
        {
            int damage = damageDto.Damage;
            
            bool doubleDamage = Random.Range(0, 2) > 0;

            damageDto.Damage = doubleDamage ? damage * 2 : damage;
            if (doubleDamage)
                Debug.Log("Critical!");
        }
    }
}