using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Handlers
{
    public class MoveHandler
    {
        public event Action<GameObject> OnTrigger;
        public event Action<GameObject> OnCollision;


        protected readonly Rigidbody2D _body;
        protected readonly Settings _stats;
        protected Vector2 _pausedLinearVelocity = Vector2.zero;
        protected float _pausedAngularVelocity = 0f;


        protected Vector2 Velocity
        {
            get => _body.linearVelocity;
            set => _body.linearVelocity = value;
        }

        public MoveHandler(Rigidbody2D body, Settings stats)
        {
            _body = body;
            _stats = stats;
            body.OnTriggerEnter2DAsObservable().Subscribe(InvokeTrigger);
            body.OnCollisionEnter2DAsObservable().Subscribe(InvokeCollision);
        }

        public virtual void Move(Vector2 speedMultiplier)
        {
            Velocity = _stats.Speed * speedMultiplier;
        }


        public virtual void Stop()
        {
            if (_body == null)
                return;
            _body.linearVelocity = Vector2.zero;
            _body.angularVelocity = 0;
        }

        public virtual void Pause()
        {
            if (_body == null) return;
            _pausedLinearVelocity = _body.linearVelocity;
            _pausedAngularVelocity = _body.angularVelocity;
            _body.linearVelocity = Vector2.zero;
            _body.angularVelocity = 0;
        }

        public virtual void Continue()
        {
            if (_body == null) return;
            _body.linearVelocity = _pausedLinearVelocity;
            _body.angularVelocity = _pausedAngularVelocity;
            _pausedLinearVelocity = Vector2.zero;
            _pausedAngularVelocity = 0;
        }

        private void InvokeTrigger(Collider2D collision)
            => OnTrigger?.Invoke(collision.gameObject);

        private void InvokeCollision(Collision2D collisionObject)
            => OnCollision?.Invoke(collisionObject.gameObject);

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public float Speed { get; protected set; }
        }
    }
}