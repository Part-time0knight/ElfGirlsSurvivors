using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class Finish : IState
    {
        private readonly IWindowFsm _windowFsm;

        public Finish(IWindowFsm windowFsm)
        {
            _windowFsm = windowFsm;
        }

        public void OnEnter()
        {
            _windowFsm.OpenWindow(typeof(FinishView), inHistory: true);
        }

        public void OnExit()
        {
            _windowFsm.CloseWindow();
        }
    }
}