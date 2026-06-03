using UnityEngine;

namespace Game.Infrastructure
{
    public class Signals
    {

        public struct EnemySpawn { }

        public struct EnemyDeath { }

        public struct PlayerMakeDamage
        {
            public int Damage;
        }

        public struct PlayerHealth
        {
            public int Health;
        }

        public struct AddScore
        {
            public int Score;
        }
    }
}