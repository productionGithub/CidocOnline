using Newtonsoft.Json;

namespace CidocOnline2022.Core.Utils
{
    public static class JSON
    {
        public static bool TryDeserialize<T>(string json, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            catch // (Exception e)
            {
                UnityEngine.Debug.LogError("[JSON] Deserialization failed !");
                result = default(T);
                return false;
            }
        }

        public static bool TrySerialize(object obj, out string json, bool indent = true)
        {
            Formatting formatting = indent ? Formatting.Indented : Formatting.None;
            try
            {
                json = JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (string.IsNullOrWhiteSpace(json))
                {
                    return false;
                }
                return true;
            }
            catch // (Exception e)
            {
                UnityEngine.Debug.LogError("[JSON] Serialization failed !");
                json = string.Empty;
                return false;
            }
        }
    }
}
