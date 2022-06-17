using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardInstaller : MonoInstaller
    {

        [SerializeField] private CardController _cardController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle();
            Container.Bind<CardController>().FromInstance(_cardController);
        }
    }
}