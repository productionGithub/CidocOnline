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
            Debug.Log("Installer is called");
            //Container.BindInterfacesAndSelfTo<DecksController>().AsSingle().NonLazy();
            Container.Bind<BoardController>().FromInstance(_boardController).AsSingle();
            Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle();

        }
    }
}