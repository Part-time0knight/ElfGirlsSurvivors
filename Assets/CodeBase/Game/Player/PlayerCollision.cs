using Game.Handlers;
using Game.Units;
using UnityEngine;

namespace Game.Player
{
    public class PlayerCollision
    {
        private readonly PlayerFacade _player;

        public PlayerCollision(PlayerFacade unit)
        {
            _player = unit;
        }

        public void MakeCollision(UnitFacade target)
        {
            target.MakeCollision(new() { DamageDealer = _player });
        }
    }
}