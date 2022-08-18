using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Localization;
using StarterCore.Core.Scenes.Form;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninInstaller : MonoInstaller
    {
        [SerializeField] private SigninController _signinController;
        [SerializeField] private LocalizationManager _localization;
        //[SerializeField] private FormController _FormController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SigninManager>().AsSingle();
            Container.Bind<SigninController>().FromInstance(_signinController);
        }
    }
}