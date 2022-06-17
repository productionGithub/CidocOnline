using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Board
{
    public class TickerInstaller : MonoInstaller
    {

        [SerializeField] private TickerController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TickerManager>().AsSingle();
            Container.Bind<TickerController>().FromInstance(_controller);
        }
    }
}