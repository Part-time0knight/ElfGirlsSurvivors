using Core.Infrastructure.GameFsm;
using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;
using Game.Enemies;
using Game.Player;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class Initialize : IState
    {
        private readonly IGameStateMachine _fsm;
        private readonly IWindowResolve _windowResolve;

        public Initialize(IGameStateMachine fsm,
            IWindowResolve windowResolve) 
        {
            _fsm = fsm;
            _windowResolve = windowResolve;
        }
        
        public void OnEnter()
        {
            WindowsResolve();
            _fsm.Enter<Idle>();
        }

        public void OnExit()
        {
        }

        private void WindowsResolve()
        {
            _windowResolve.CleanUp();
            _windowResolve.Set<FinishView>();
            _windowResolve.Set<HUDView>();
        }
    }
}