using UnityEngine;
using Zenject;

using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardInstaller : MonoInstaller
    {

        [SerializeField] private BoardController _boardController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle();
            Container.Bind<BoardController>().FromInstance(_boardController);
        }
    }
}