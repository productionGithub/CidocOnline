using System;
using Cysharp.Threading.Tasks;
using StarterCore.Core.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace StarterCore.Core.Services.Network
{
    public class NetworkService
    {
        public async UniTask<String> GetRawStringAsync<T>(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {
                    await webRequest.SendWebRequest();

                    string content = webRequest.downloadHandler.text;
                    //Debug.Log("Content returned =====> " + content);

                    return content;// ParseResult<T>(webRequest);
                }
            }
            else
            {
                Debug.LogError("[NetworkService] GET Given url is empty.");
            }
            return default;
        }

        public async UniTask<T> GetAsync<T>(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {
                    await webRequest.SendWebRequest();

                    string content = webRequest.downloadHandler.text;
                    Debug.Log("Content returned =====> " + content);

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
                    using (UnityWebRequest webRequest = UnityWebRequest.Put(url, json))
                    {
                        webRequest.method = "POST";
                        webRequest.SetRequestHeader("Content-Type", "application/json");
                        //webRequest.SetRequestHeader("Accept", "application/json");

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
                //Debug.Log("Request is done ? -> " + webRequest.isDone); // True.
                string content = webRequest.downloadHandler.text;
                //Debug.Log("WebRequest content iii: " + webRequest.downloadHandler.text);

                if (JSON.TryDeserialize<T>(content, out T parsed))
                {
                    //Debug.Log("Parsed Json response -> " + parsed.ToString());
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

        public static string FormatByteArray(byte[] aData)
        {
            if (aData == null)
                return "byte array is null";
            var sb = new System.Text.StringBuilder();
            sb.Append("byte array ( length: ").Append(aData.Length).Append(") ").AppendLine();
            int count = 0;
            foreach (var b in aData)
            {
                sb.AppendFormat("{0,2:X2} ", b);
                if (++count % 16 == 0)
                    sb.AppendLine();
            }
            return sb.ToString();
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
