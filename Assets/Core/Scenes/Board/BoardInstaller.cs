using UnityEngine;
using Zenject;
using StarterCore.Core.Scenes.Board.Challenge;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private BoardController _boardController;
        [SerializeField] private ChallengeEvaluator _challengeEvaluator;

        public override void InstallBindings()
        {
            //Container.BindInterfacesAndSelfTo<InstanceDeckService>().AsSingle().NonLazy();
            Container.Bind<BoardController>().FromInstance(_boardController).AsSingle();
            Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle();
            Container.Bind<ChallengeEvaluator>().FromInstance(_challengeEvaluator).AsSingle();
        }
    }
}