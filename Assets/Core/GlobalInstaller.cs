using StarterCore.Core.Services.Network;
using UnityEngine;
using Zenject;

namespace StarterCore.Core
{
    [CreateAssetMenu(fileName = "GlobalInstaller", menuName = "StarterCore/GlobalInstaller", order = 0)]
    public class GlobalInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkService>().AsSingle();
            Container.Bind<MockNetService>().AsSingle();
        }
    }
}
