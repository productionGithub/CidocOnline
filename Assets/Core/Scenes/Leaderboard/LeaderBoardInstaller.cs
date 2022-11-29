using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LeaderBoardInstaller : MonoInstaller
    {
        [SerializeField] private LeaderBoardController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LeaderBoardManager>().AsSingle();
            Container.Bind<LeaderBoardController>().FromInstance(_controller);
        }
    }
}