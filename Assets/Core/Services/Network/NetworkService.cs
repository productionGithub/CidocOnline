using Cysharp.Threading.Tasks;
using StarterCore.Core.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace StarterCore.Core.Services.Network
{
    public class NetworkService
    {
        public async UniTask<T> GetAsync<T>(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {
                    await webRequest.SendWebRequest();

                    string content = webRequest.downloadHandler.text;
                    Debug.Log(content);

                    return ParseResult<T>(webRequest);
                }
            }
            else
            {
                Debug.LogError("[NetworkService] GET Given url is empty.");
            }
            return default;
        }

        public async UniTask<T> PostAsync<T>(string url, object data)
        {
            if(!string.IsNullOrWhiteSpace(url) && data != null)
            {
                if(JSON.TrySerialize(data, out string json, indent: false))
                {
                    using (UnityWebRequest webRequest = UnityWebRequest.Post(url, json))
                    {
                        await webRequest.SendWebRequest();
                        return ParseResult<T>(webRequest);
                    }
                }
                else
                {
                    Debug.LogError("[NetworkService] Impossible to serialize given object.");
                }
            }
            else
            {
                Debug.LogError("[NetworkService] POST Given url is empty.");
            }
            return default;
        }

        private T ParseResult<T>(UnityWebRequest webRequest)
        {
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string content = webRequest.downloadHandler.text;
                if (JSON.TryDeserialize<T>(content, out T parsed))
                {
                    return parsed;
                }
                else
                {
                    Debug.LogError("[NetworkService] Web result parsing failed.");
                }
            }
            else
            {
                Debug.LogError("[NetworkService] Web request failed.");
            }
            return default;
        }

        // Kept as example for pure C# version, doesn't work on WebGL !
        //private HttpClient _http;
        //public async UniTask<T> GetPureAsync<T>(string url)
        //{
        //    if (!string.IsNullOrWhiteSpace(url))
        //    {
        //        HttpResponseMessage response = await _http.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string content = await response.Content.ReadAsStringAsync();
        //            if (JSON.TryDeserialize<T>(content, out T parsed))
        //            {
        //                return parsed;
        //            }
        //        }
        //    }
        //    return default;
        //}
    }
}
