using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.ResetPassword
{
    public class ResetPasswordInstaller : MonoInstaller
    {
        [SerializeField] private ResetPasswordController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ResetPasswordManager>().AsSingle();
            Container.Bind<ResetPasswordController>().FromInstance(_controller);
        }
    }
}