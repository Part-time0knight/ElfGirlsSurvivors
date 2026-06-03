using Core.Infrastructure.GameFsm.States;
using Core.MVVM.Windows;
using Game.InteractiveObjects;
using Game.Player;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class Idle : IState
    {
        private readonly PlayerFacade.Pool _playerPool;
        private readonly BattleZoneEnter.Pool _enterPool;
        private readonly IWindowFsm _windowFsm;

        private PlayerFacade _player;
        private BattleZoneEnter _battleZone;

        public Idle(PlayerFacade.Pool playerPool,
            BattleZoneEnter.Pool enterPool,
            IWindowFsm windowFsm)
        {
            _playerPool = playerPool;
            _enterPool = enterPool;
            _windowFsm = windowFsm;
        }

        public void OnEnter()
        {
            _player = _playerPool.Spawn();
            _player.EnterIdle();
            _battleZone = _enterPool.Spawn();
            _windowFsm.OpenWindow(typeof(HUDView));
        }

        public void OnExit()
        {
            _playerPool.Despawn(_player);
            _enterPool.Despawn(_battleZone);
            _windowFsm.CloseWindow(typeof(HUDView));
        }
    }
}