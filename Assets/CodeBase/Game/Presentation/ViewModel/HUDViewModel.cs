using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using Game.Presentation.View;
using Game.Services;
using System;
using UnityEngine;

namespace Game.Presentation.ViewModel
{
    public class HUDViewModel : AbstractViewModel
    {
        public event Action<string> OnUpdate;

        protected override Type Window => typeof(HUDView);

        private PlayerDamageService _playerService;

        public HUDViewModel(IWindowFsm windowFsm,
            PlayerDamageService playerService) : base(windowFsm)
        {
            _playerService = playerService;
        }

        public override void InvokeClose()
        {
            _windowFsm.OpenWindow(Window);
        }

        public override void InvokeOpen()
        {
            _windowFsm.CloseWindow(Window);
        }

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);
            if (Window != uiWindow) return;

            OnUpdate?.Invoke(_playerService.Health.ToString());
            _playerService.OnHealthUpdate += InvokeUpdate;
        }

        protected override void HandleClosedWindow(Type uiWindow)
        {
            base.HandleClosedWindow(uiWindow);
            if (Window != uiWindow) return;

            _playerService.OnHealthUpdate -= InvokeUpdate;
        }

        private void InvokeUpdate()
        {
            OnUpdate?.Invoke(_playerService.Health.ToString());
        }
    }
}