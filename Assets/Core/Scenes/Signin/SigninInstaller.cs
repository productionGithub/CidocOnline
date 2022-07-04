using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninInstaller : MonoInstaller
    {
        [SerializeField] private SigninController _controller;

        public override void InstallBindings()
        {
            //Container.BindInterfacesAndSelfTo<SignupManager>().AsSingle();
            //Container.Bind<SignupController>().FromInstance(_controller);
        }
    }
}