using Cysharp.Threading.Tasks;
using CidocOnline2022.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;

namespace CidocOnline2022.Core.Services.Network
{
    public class MockNetService
    {
        // More examples data here : https://random-data-api.com/documentation
        private const string URL_FORMAT = "https://random-data-api.com/api/users/random_user?size={0}";

        [Inject] private NetworkService _net;

        public async UniTask<List<User>> GetRandomUsersAsync(int count)
        {
            string url = string.Format(URL_FORMAT, count);
            List<User> result = await _net.GetAsync<List<User>>(url);
            return result;
        }
    }
}
