using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Localization;


namespace StarterCore.Core.Scenes.ResetPassword
{
    public class ResetPasswordInstaller : MonoInstaller
    {
        [SerializeField] private ResetPasswordController _controller;
        [SerializeField] private LocalizationController _localizationController;

        public override void InstallBindings()
        {
            Container.Bind<LocalizationController>().FromInstance(_localizationController);
            Container.BindInterfacesAndSelfTo<ResetPasswordManager>().AsSingle();
            Container.Bind<ResetPasswordController>().FromInstance(_controller);
        }
    }
}