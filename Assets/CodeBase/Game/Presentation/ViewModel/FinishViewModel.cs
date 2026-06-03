using Core.Infrastructure.GameFsm;
using Core.MVVM.ViewModel;
using Core.MVVM.Windows;
using Game.Infrastructure.States;
using Game.Presentation.View;
using Game.Services;
using System;
using UnityEngine;

namespace Game.Presentation.ViewModel
{
    public class FinishViewModel : AbstractViewModel
    {
        public event Action<Dto> OnShow;

        protected override Type Window => typeof(FinishView);

        private readonly PlayerDamageService _playerDamage;
        private readonly EnemyCounterService _enemyCounter;
        private readonly ScoreService _scoreService;
        private readonly IGameStateMachine _fsm;

        public FinishViewModel(IWindowFsm windowFsm,
            PlayerDamageService playerDamage,
            EnemyCounterService enemyCounter,
            ScoreService scoreService,
            IGameStateMachine fsm)
            : base(windowFsm)
        {
            _playerDamage = playerDamage;
            _enemyCounter = enemyCounter;
            _scoreService = scoreService;
            _fsm = fsm;
        }

        public override void InvokeClose()
        {
            _windowFsm.OpenWindow(Window, inHistory: true);
        }

        public override void InvokeOpen()
        {
            _windowFsm.CloseWindow();
        }

        public void Restart()
        {
            _fsm.Enter<Idle>();
        }

        protected override void HandleOpenedWindow(Type uiWindow)
        {
            base.HandleOpenedWindow(uiWindow);
            if (Window != uiWindow) return;
            InvokeShow();
        }

        private void InvokeShow()
        {
            Dto dto = new Dto()
            {
                IsWin = _playerDamage.Health > 0,
                Statistic = $"Enemies killed: {_enemyCounter.Killed}\n"
                    + $"Player damage dealt: {_playerDamage.MakeDamage}\n"
                    + $"Current score: {_scoreService.Score}"
            };

            OnShow?.Invoke(dto);
        }


        public class Dto
        {
            public bool IsWin;
            public string Statistic;
        }
    }
}