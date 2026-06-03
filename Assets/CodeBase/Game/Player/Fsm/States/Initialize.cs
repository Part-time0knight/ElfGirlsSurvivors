using Core.Infrastructure.GameFsm;
using Core.Infrastructure.GameFsm.States;
using Game.Handlers;
using UnityEngine;

namespace Game.Player.Fsm.States
{
    public class Initialize : IState
    {
        private readonly PlayerDamageHandler _damageHandler;

        public Initialize(PlayerDamageHandler damageHandler)
        {
            _damageHandler = damageHandler;
        }

        public void OnEnter()
        {
            _damageHandler.Reset();
        }

        public void OnExit()
        {
            
        }
    }
}