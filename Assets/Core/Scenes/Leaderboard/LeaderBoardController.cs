#define TRACE_ON
using UnityEngine;
using StarterCore.Core.Services.Network.Models;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LeaderBoardController : MonoBehaviour
    {

        [SerializeField] LeaderBoardDisplayer _leaderBoardDisplayer;
        [SerializeField] LanguageTitleEntry _languageTitleTemplate;
        [SerializeField] LanguageTitleDivider _languageDividerTemplate;
        [SerializeField] ScenarioData _scenarioTitleTemplate;
        [SerializeField] PlayerDatum _userLineTemplate;
        [SerializeField] Button _mainMenuButton;
        [SerializeField] public GameObject _waitingIcon;

        public event Action OnBackButtonClicked;

        Transform parent;

        // Start is called before the first frame update
        public void Init()
        {
            //TODO DON'T FORGET ADDING ENTRIES!
            _leaderBoardDisplayer.Init();
            _mainMenuButton.onClick.AddListener(OnBack);

            parent = _leaderBoardDisplayer._leaderBoardContainer;
        }

        public void ShowWaitingIcon()
        {
            _waitingIcon.SetActive(true);
        }
        public void HideWaitingIcon()
        {
            _waitingIcon.SetActive(false);
        }


        public void Show(RankingModelDown rankings)
        {
            //TODO DON'T FORGET ADDING ENTRIES!
            //For each language
            foreach (Language language in rankings.Languages)
            {
                //Language Title
                LanguageTitleEntry languageTitleInstance = Instantiate(_languageTitleTemplate, parent);
                languageTitleInstance.gameObject.SetActive(true);
                languageTitleInstance._titleTxt.text = language.LanguageName;

                //For each scenario in language
                foreach (ScenarioData scenarioDatum in language.Scenarii.Scenario)
                {
                    //First sort playerData by score, descending
                    List<PlayerDatum> SortedList = scenarioDatum.PlayerData.OrderByDescending(o => o.Score).ToList();

                    Debug.Log("ScenarioName : " + scenarioDatum.ScenarioName);
                    Debug.Log("Max Score Scenario : " + scenarioDatum.MaximumScore);

                    ScenarioData scenarioData = Instantiate(_scenarioTitleTemplate, parent);
                    scenarioData.gameObject.SetActive(true);
                    scenarioData.Show(scenarioDatum);

                    int index = 0;
                    //For each playerData in scenario
                    foreach (PlayerDatum playerData in SortedList)//scenarioDatum.PLayerData
                    {
                        PlayerDatum playerInfo = Instantiate(_userLineTemplate, parent);
                        playerInfo.gameObject.SetActive(true);

                        playerInfo.Username = playerData.Username;
                        playerInfo.Score = playerData.Score;
                        playerInfo.Show(++index, playerData, scenarioDatum);
                    }
                }
                //Language divider
                LanguageTitleDivider dividerInstance = Instantiate(_languageDividerTemplate, parent);
                dividerInstance.gameObject.SetActive(true);
            }
        }

        private void OnBack()
        {
            OnBackButtonClicked?.Invoke();
        }

        private void OnDestroy()
        {
            _mainMenuButton.onClick.RemoveListener(OnBack);
        }
    }
}




