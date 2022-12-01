using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Localization;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninInstaller : MonoInstaller
    {
        [SerializeField] private SigninController _signinController;
        [SerializeField] private LocalizationManager _localization;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SigninManager>().AsSingle();
            Container.Bind<SigninController>().FromInstance(_signinController);
        }
    }
}