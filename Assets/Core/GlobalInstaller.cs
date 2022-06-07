using CidocOnline2022.Core.Services.Network;
using UnityEngine;
using Zenject;

namespace CidocOnline2022.Core
{
    [CreateAssetMenu(fileName = "GlobalInstaller", menuName = "StarterCore/GlobalInstaller", order = 0)]
    public class GlobalInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NetworkService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MockNetService>().AsSingle();
        }
    }
}
