using UnityEngine;
using Zenject;


namespace StarterCore.Core.Scenes.GameSelection
{
    public class GameSelectionInstaller : MonoInstaller
    {
        [SerializeField] private GameSelectionController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSelectionManager>().AsSingle();
            Container.Bind<GameSelectionController>().FromInstance(_controller);
        }
    }
}