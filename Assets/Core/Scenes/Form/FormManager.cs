using Cysharp.Threading.Tasks;
using CidocOnline2022.Core.Services.Network;
using CidocOnline2022.Core.Services.Network.Models;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CidocOnline2022.Core.Scenes.Form
{
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
            List<User> users = await _net.GetRandomUsersAsync(5);

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
