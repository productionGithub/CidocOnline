using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class ScenarioData : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scenarioName;

        public string ScenarioName { get; set; }
        public int MaximumScore { get; set; }
        public List<PlayerDatum> PlayerData { get; set; }

        public void Show(ScenarioData data)
        {
            _scenarioName.text = $"{data.ScenarioName}";
        }
    }
}