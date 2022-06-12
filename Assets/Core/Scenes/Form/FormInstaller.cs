using UnityEngine;
using Zenject;

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
