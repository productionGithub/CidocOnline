using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Localization;

namespace StarterCore.Core.Scenes.Form
{
    public class FormInstaller : MonoInstaller
    {
        [SerializeField] private FormController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FormManager>().AsSingle();
            Container.Bind<FormController>().FromInstance(_controller);
        }
    }
}
