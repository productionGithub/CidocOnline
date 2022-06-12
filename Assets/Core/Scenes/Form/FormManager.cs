using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Form
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class FormManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private FormController _controller;

        public void Initialize()
        {
            _controller.OnRefreshClickedEvent += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            FetchAsync().Forget();
        }

        private async UniTaskVoid FetchAsync()
        {
            List<User> users = await _net.GetRandomUsersAsync("God");

            if(users != null && users.Count > 0)
            {
                _controller.Show(users);
            }
            else
            {
                Debug.Log("[FormManager] Failed to fetch users !");
            }
        }
    }
}
