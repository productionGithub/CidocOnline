using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Localization;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninInstaller : MonoInstaller
    {
        [SerializeField] private SigninController _controller;
        [SerializeField] private LocalizationController _localizationController;

        public override void InstallBindings()
        {
            Container.Bind<LocalizationController>().FromInstance(_localizationController);
            Container.BindInterfacesAndSelfTo<SigninManager>().AsSingle();
            Container.Bind<SigninController>().FromInstance(_controller);
        }
    }
}