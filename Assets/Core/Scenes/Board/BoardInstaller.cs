using UnityEngine;
using Zenject;

using StarterCore.Core.Services.GameState;
using StarterCore.Core.Scenes.Board.Challenge;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardInstaller : MonoInstaller
    {

        [SerializeField] private BoardController _boardController;
        [SerializeField] private ChallengeEvaluator _challengeEvaluator;

        public override void InstallBindings()
        {
            Debug.Log("Installer is called");
            Container.BindInterfacesAndSelfTo<InstanceDeckService>().AsSingle().NonLazy();
            Container.Bind<BoardController>().FromInstance(_boardController).AsSingle();
            Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle();
            Container.Bind<ChallengeEvaluator>().FromInstance(_challengeEvaluator).AsSingle();
        }
    }
}