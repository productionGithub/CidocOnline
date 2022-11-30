using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Localization;


namespace StarterCore.Core.Scenes.GoingFurther
{
    public class GoingFurtherInstaller : MonoInstaller
    {
        [SerializeField] private GoingFurtherController _controller;

        public override void InstallBindings()
        {
            Debug.Log("GF Installer Binding");
            Container.BindInterfacesAndSelfTo<GoingFurtherManager>().AsSingle();
            Container.Bind<GoingFurtherController>().FromInstance(_controller);
        }
    }
}