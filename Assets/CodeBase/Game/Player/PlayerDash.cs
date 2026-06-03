using Game.Misc;
using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerDash
    {
        private readonly Timer _timerDelay = new();
        private readonly Timer _timerDuration = new();
        private readonly Rigidbody2D _body;
        private readonly Settings _settings;

        public PlayerDash(Rigidbody2D body,
            Settings settings)
        {
            _body = body;
            _settings = settings;
        }

        public void Dash()
        {
            if (_timerDelay.Active) return;

            _timerDelay.Initialize(_settings.Delay).Play();
            _timerDuration
                .Initialize(_settings.DashDuration, Time.fixedDeltaTime, Move, null)
                .Play();
        }

        private void Move(float tick)
        {
            _body.MovePosition(_body.position 
                + _body.linearVelocity.normalized
                * _settings.Distance 
                * Time.fixedDeltaTime / _settings.DashDuration);
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public float Distance { get; private set; }
            [field: SerializeField] public float Delay { get; private set; }
            [field: SerializeField] public float DashDuration { get; private set; }
        }
    }
}