using TMPro;
using UnityEngine;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class PlayerDatum : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _rank;
        [SerializeField] TextMeshProUGUI _userName;
        [SerializeField] TextMeshProUGUI _score;
        [SerializeField] TextMeshProUGUI _maxScenarioscore;
        [SerializeField] TextMeshProUGUI _ratio;

        public string Username { get; set; }
        public int Score { get; set; }

        public void Show(int index, PlayerDatum playerData, ScenarioData scenarioData)
        {
            _rank.text = index.ToString();
            _userName.text =  $"{playerData.Username}";
            _score.text = $"{playerData.Score}";
            _maxScenarioscore.text = $"{scenarioData.MaximumScore}";
            _ratio.text = (playerData.Score * 100 / scenarioData.MaximumScore)+"%";
        }
    }
}
