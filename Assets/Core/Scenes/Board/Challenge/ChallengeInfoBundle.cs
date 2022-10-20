namespace StarterCore.Core.Scenes.Board.Challenge
{
    public class ChallengeInfoBundle
    {
        public string ScenarioTitle;
        public string ChapterTitle;
        public int ChallengeIndex;

        public ChallengeInfoBundle(string scenarioTitle, string chapterTitle, int challengeIndex)
        {
            ScenarioTitle = scenarioTitle;
            ChapterTitle = chapterTitle;
            ChallengeIndex = challengeIndex;
        }
    }
}
