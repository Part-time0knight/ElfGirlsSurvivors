using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<Signals.EnemySpawn>();
            Container.DeclareSignal<Signals.EnemyDeath>();
            Container.DeclareSignal<Signals.PlayerMakeDamage>();
            Container.DeclareSignal<Signals.PlayerHealth>();
            Container.DeclareSignal<Signals.AddScore>();
        }
    }
}