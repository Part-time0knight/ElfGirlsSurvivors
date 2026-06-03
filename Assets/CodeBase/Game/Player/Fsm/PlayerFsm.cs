using Game.Player.Fsm.States;
using Core.Domain.Factories;
using Core.Infrastructure.GameFsm;
using UnityEngine;
using Zenject;

namespace Game.Player.Fsm
{
    public class PlayerFsm : AbstractGameStateMachine, IInitializable
    {
        public PlayerFsm(IStatesFactory factory) : base(factory)
        {
        }

        public void Initialize()
        {
            StateResolve();
        }

        private void StateResolve()
        {
            _states.Add(typeof(Initialize), _factory.Create<Initialize>());
            _states.Add(typeof(Battle), _factory.Create<Battle>());
            _states.Add(typeof(Idle), _factory.Create<Idle>());
            _states.Add(typeof(Deactive), _factory.Create<Deactive>());
        }
    }
}