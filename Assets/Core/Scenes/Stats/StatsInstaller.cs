using UnityEngine;
using Zenject;

using StarterCore.Core.Services.GameState;
using StarterCore.Core.Scenes.Board.Challenge;
using StarterCore.Core.Scenes.Stats;

namespace StarterCore.Core.Scenes.Board
{
    public class StatsInstaller : MonoInstaller
    {
        [SerializeField] private StatsController _controller;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StatsManager>().AsSingle();
            Container.Bind<StatsController>().FromInstance(_controller);
        }
    }
}