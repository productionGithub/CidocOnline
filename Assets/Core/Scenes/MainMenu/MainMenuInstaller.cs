using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuManager>().AsSingle();
            Container.Bind<MainMenuController>().FromInstance(_controller);
        }
    }
}