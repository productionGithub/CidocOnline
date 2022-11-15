using Newtonsoft.Json;

public class UpdateSessionModelUp
{
    [JsonProperty("userId")]
    public int UserId;//int

    [JsonProperty("currentScenario")]
    public string CurrentScenario;

    [JsonProperty("currentChapter")]
    public string CurrentChapter;

    [JsonProperty("currentChallengeIndex")]
    public int CurrentChallengeIndex;

    [JsonProperty("currentScore")]
    public int CurrentScore;
}
