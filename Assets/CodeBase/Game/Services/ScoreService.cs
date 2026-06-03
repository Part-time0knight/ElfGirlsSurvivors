using Game.Infrastructure;
using System;
using UnityEngine;
using Zenject;

namespace Game.Services
{
    public class ScoreService : IInitializable, IDisposable
    {
        public int Score => _score;

        private readonly SignalBus _signalBus;
        private readonly SaveService _saveService;

        private int _score = 0;

        public ScoreService(SignalBus signalBus,
            SaveService saveService)
        {
            _signalBus = signalBus;
            _saveService = saveService;
        }

        public void Initialize()
        {
            _signalBus
                .Subscribe<Signals.AddScore>(InvokeAddScore);
            _saveService.Load();
        }

        public void Dispose()
        {
            _signalBus
                .Unsubscribe<Signals.AddScore>(InvokeAddScore);
        }

        private void InvokeAddScore(Signals.AddScore signal)
        {
            _score += signal.Score;
            _saveService.Save();
        }
    }
}