using Game.Infrastructure;
using System;
using UnityEngine;
using Zenject;

namespace Game.Services
{
    public class PlayerDamageService : IInitializable, IDisposable
    {
        public event Action OnHealthUpdate;

        public int MakeDamage => _damage;

        public int Health => _health;

        private readonly SignalBus _signalBus;

        private int _damage = 0;

        private int _health = 0;

        public PlayerDamageService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }


        public void Initialize()
        {
            _signalBus
                .Subscribe<Signals.PlayerMakeDamage>(InvokeMakeDamage);

            _signalBus
                .Subscribe<Signals.PlayerHealth>(InvokeHealth);
        }

        public void Dispose()
        {
            _signalBus
                .Unsubscribe<Signals.PlayerMakeDamage>(InvokeMakeDamage);

            _signalBus
                .Unsubscribe<Signals.PlayerHealth>(InvokeHealth);
        }

        public void Reset()
        {
            _damage = 0;
        }

        private void InvokeHealth(Signals.PlayerHealth health)
        {
            _health = health.Health;
            OnHealthUpdate?.Invoke();
        }

        private void InvokeMakeDamage(Signals.PlayerMakeDamage damage)
        {
            _damage += damage.Damage;
        }
    }
}