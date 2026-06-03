using Core.Domain.Factories;
using Core.Infrastructure.GameFsm;
using Game.Infrastructure.States;
using Zenject;

namespace Game.Infrastructure
{
    public class GameFsm : AbstractGameStateMachine, IInitializable
    {
        public GameFsm(IStatesFactory factory) : base(factory)
        {
        }

        public void Initialize()
        {
            StateResolve();
            Enter<Initialize>();
        }

        private void StateResolve()
        {
            _states.Add(typeof(Initialize), _factory.Create<Initialize>());
            _states.Add(typeof(Battle), _factory.Create<Battle>());
            _states.Add(typeof(Finish), _factory.Create<Finish>());
            _states.Add(typeof(Idle), _factory.Create<Idle>());
        }
    }
}