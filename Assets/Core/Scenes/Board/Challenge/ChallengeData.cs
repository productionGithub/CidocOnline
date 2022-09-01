using Newtonsoft.Json;
using System;
using System.Collections.Generic;
/// <summary>
/// This class is used to de/serialize Challenge data:
/// </summary>

namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class ChallengeData
    {
    /*
    //Constructors
    public ChallengeData() { }

    public ChallengeData(ChallengeData obj)
    {
        ChallengeID = obj.ChallengeID;
        Title = obj.Title;
        Statement = obj.Statement;
        ILeftInit = obj.ILeftInit;
        IMiddleInit = obj.IMiddleInit;
        IRightInit = obj.IRightInit;
        ELeftInit = obj.ELeftInit;
        PLeftInit = obj.PLeftInit;
        EMiddleInit = obj.EMiddleInit;
        PRightInit = obj.PRightInit;
        ERightInit = obj.ERightInit;
        Explanation = obj.Explanation;
        ELeftAnswer = obj.ELeftAnswer;
        PLeftAnswer = obj.PLeftAnswer;
        EMiddleAnswer = obj.EMiddleAnswer;
        PRightAnswer = obj.PRightAnswer;
        ERightAnswer = ERightAnswer;
        Score = obj.Score;
    }
*/
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

/*

    //Get list of Ids from the game data file string
    public string[] GetArrayOfValues(string propertyValue)//-E18, E29, ...
    {
        string[] listOfIds = null;

        if (propertyValue != "")//string.Empty ou mieux !string.isNullOrWhitespace
        {
            //Create an array of ids from comma separated string of ids
            string trimFirstChar = propertyValue.TrimStart('-');//E18
            string cleanString = String.Concat(trimFirstChar.Where(c => !Char.IsWhiteSpace(c)));//Remove all whitespaces
            listOfIds = cleanString.Split(',');
        }
        return listOfIds;//(E18,E29)
    }
    */
    }
}


