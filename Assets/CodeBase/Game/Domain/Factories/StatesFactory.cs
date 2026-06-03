using Core.Domain.Factories;
using Core.Infrastructure.GameFsm.States;
using Zenject;

namespace Game.Domain.Factories
{
    public class StatesFactory : IStatesFactory
    {
        private readonly DiContainer _container;

        public StatesFactory(DiContainer container) =>
            _container = container;

        public virtual TState Create<TState>() where TState : class, IExitableState
        {
            TState state = _container.Instantiate<TState>();
            return state;
        }
    }
}
