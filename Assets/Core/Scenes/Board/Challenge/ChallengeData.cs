using Newtonsoft.Json;
using System;

/// <summary>
/// This class is used to de/serialize Challenge data:
/// </summary>

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ChallengeData
    {
    //Props
    [JsonProperty("ChallengeID")]
    public int ChallengeID { get; set; }
    [JsonProperty("Title")]
    public string Title { get; set; }
    [JsonProperty("Statement")]
    public string Statement { get; set; }
    [JsonProperty("ILeftInit")]
    public string ILeftInit { get; set; }
    [JsonProperty("IMiddleInit")]
    public string IMiddleInit { get; set; }
    [JsonProperty("IRightInit")]
    public string IRightInit { get; set; }
    [JsonProperty("ELeftInit")]
    public string ELeftInit { get; set; }
    [JsonProperty("PLeftInit")]
    public string PLeftInit { get; set; }
    [JsonProperty("EMiddleInit")]
    public string EMiddleInit { get; set; }
    [JsonProperty("PRightInit")]
    public string PRightInit { get; set; }
    [JsonProperty("ERightInit")]
    public string ERightInit { get; set; }
    [JsonProperty("Explanation")]    
    public string Explanation { get; set; }
    [JsonProperty("ELeftAnswer")]   
    public string ELeftAnswer { get; set; }
    [JsonProperty("PLeftAnswer")]   
    public string PLeftAnswer { get; set; }
     [JsonProperty("EMiddleAnswer")]      
    public string EMiddleAnswer { get; set; }
     [JsonProperty("PRightAnswer")]
    public string PRightAnswer { get; set; }
    [JsonProperty("ERightAnswer")]
    public string ERightAnswer { get; set; }
    [JsonProperty("Score")]
    public int Score { get; set; }
    }
}


