using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace StarterCore.Core.Utils
{
    public static class ImageDownloader
    {
        public static async UniTask<Texture2D> DownloadImage(string url, string imageName, string imageFormat)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Error in downloading image : {request.error}");
                    return null;
                }
                else
                {
                    Texture2D myTexture = DownloadHandlerTexture.GetContent(request);
                    return myTexture;
                }
            }
        }
    }
}