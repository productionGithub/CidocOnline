using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class _ScenariiModelDown : MonoBehaviour
    {
        [JsonProperty("scenarii")]
        public List<_ScenariiModelDown> Calalog { get; set; }
    }
}