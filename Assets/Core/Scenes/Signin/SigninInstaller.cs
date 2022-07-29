using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninInstaller : MonoInstaller
    {
        [SerializeField] private SigninController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SigninManager>().AsSingle();
            Container.Bind<SigninController>().FromInstance(_controller);
        }
    }
}