using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupInstaller : MonoInstaller
    {
        [SerializeField] private SignupController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SignupManager>().AsSingle();
            Container.Bind<SignupController>().FromInstance(_controller);
        }
    }
}
