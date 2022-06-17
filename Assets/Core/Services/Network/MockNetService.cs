using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;

namespace StarterCore.Core.Services.Network
{
    public class MockNetService
    {
        // More examples data here : https://random-data-api.com/documentation

        private const string URL_FORMAT = "https://www.talgorn.me/phpTest/hello.php?name={0}";
        //private const string URL_FORMAT = "https://ontomatchgame.huma-num.fr/phpTest/hello.php?name={0}";

        [Inject] private NetworkService _net;

        public async UniTask<List<User>> GetRandomUsersAsync(string name)
        {
            var timestamp = UnityEngine.Time.time;
            string url = string.Format(URL_FORMAT, name);
            List<User> result = await _net.GetAsync<List<User>>(url);
            return result;
        }

        // More methods about the API
    }
}
