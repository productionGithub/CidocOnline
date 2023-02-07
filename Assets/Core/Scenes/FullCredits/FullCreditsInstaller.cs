using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Localization;


namespace StarterCore.Core.Scenes.FullCredits
{
    public class FullCreditsInstaller : MonoInstaller
    {
        [SerializeField] private FullCreditsController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FullCreditsManager>().AsSingle();
            Container.Bind<FullCreditsController>().FromInstance(_controller);
        }
    }
}