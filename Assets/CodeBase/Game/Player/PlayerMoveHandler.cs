using Game.Handlers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Player
{
    public class PlayerMoveHandler : MoveHandler
    {

        public PlayerMoveHandler(Rigidbody2D body, PlayerSettings stats) : base(body, stats)
        {
        }

        [Serializable]
        public class PlayerSettings : Settings
        {

        }
    }
}