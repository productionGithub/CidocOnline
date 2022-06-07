using Cysharp.Threading.Tasks;
using CidocOnline2022.Core.Utils;
using System.Net.Http;
using Zenject;

namespace CidocOnline2022.Core.Services.Network
{
    public class NetworkService : IInitializable
    {
        private HttpClient _http;

        public void Initialize()
        {
            _http = new HttpClient();
        }

        public async UniTask<T> GetAsync<T>(string url)
        {
            if(!string.IsNullOrWhiteSpace(url))
            {
                HttpResponseMessage response = await _http.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if(JSON.TryDeserialize<T>(content, out T parsed))
                    {
                        return parsed;
                    }
                }
            }
            return default;
        }
    }
}
