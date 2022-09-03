using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ScenariiModelDown
    {
        [JsonProperty("scenarii")]
        public List<Scenario> Scenarii;
    }
}
