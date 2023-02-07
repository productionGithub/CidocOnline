using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.SplashFullScreen
{
    public class SplashFullScreenInstaller : MonoInstaller
    {
        [SerializeField] private SplashFullScreenController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SplashFullScreenManager>().AsSingle();
            Container.Bind<SplashFullScreenController>().FromInstance(_controller);
        }
    }
}