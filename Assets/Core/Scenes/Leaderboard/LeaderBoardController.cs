#define TRACE_OFF
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

        private List<LanguageTitleEntry> _languageEntries;//Keeps track of HierarchyEntityEntry instances
        private List<ScenarioData> _scenarioEntries;//Keeps track of ScenarioData instances
        private List<PlayerDatum> _playerDatumEntries;//Keeps track of ScenarioData instances
        private List<LanguageTitleDivider> _languageDividerEntries;//Keeps track of ScenarioData instances

        Transform parent;

        public void Init()
        {
            //LanguageTitleEntry management
            if (_languageEntries == null)
            {
                _languageEntries = new List<LanguageTitleEntry>();
            }
            else
            {
                foreach (LanguageTitleEntry e in _languageEntries)
                {
                    Destroy(e.gameObject);
                }
                _languageEntries.Clear();
            }

            //Scenario management
            if (_scenarioEntries == null)
            {
                _scenarioEntries = new List<ScenarioData>();
            }
            else
            {
                foreach (ScenarioData e in _scenarioEntries)
                {
                    Destroy(e.gameObject);
                }
                _scenarioEntries.Clear();
            }

            //PlayerDatum management
            if (_playerDatumEntries == null)
            {
                _playerDatumEntries = new List<PlayerDatum>();
            }
            else
            {
                foreach (PlayerDatum e in _playerDatumEntries)
                {
                    Destroy(e.gameObject);
                }
                _playerDatumEntries.Clear();
            }

            //LanguageTitleDivider management
            if (_languageDividerEntries == null)
            {
                _languageDividerEntries = new List<LanguageTitleDivider>();
            }
            else
            {
                foreach (LanguageTitleDivider e in _languageDividerEntries)
                {
                    Destroy(e.gameObject);
                }
                _languageDividerEntries.Clear();
            }

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
                _languageEntries.Add(languageTitleInstance);

                //For each scenario in language
                foreach (ScenarioData scenarioDatum in language.Scenarii.Scenario)
                {
                    //First sort playerData by score, descending
                    List<PlayerDatum> SortedList = scenarioDatum.PlayerData.OrderByDescending(o => o.Score).ToList();

                    ScenarioData scenarioData = Instantiate(_scenarioTitleTemplate, parent);
                    scenarioData.gameObject.SetActive(true);
                    scenarioData.Show(scenarioDatum);
                    _scenarioEntries.Add(scenarioData);

                    int index = 0;
                    //For each playerData in scenario
                    foreach (PlayerDatum playerData in SortedList)//scenarioDatum.PLayerData
                    {
                        PlayerDatum playerInfo = Instantiate(_userLineTemplate, parent);
                        playerInfo.gameObject.SetActive(true);
                        playerInfo.Username = playerData.Username;
                        playerInfo.Score = playerData.Score;
                        _playerDatumEntries.Add(playerInfo);

                        playerInfo.Show(++index, playerData, scenarioDatum);
                    }
                }
                //Language divider
                LanguageTitleDivider dividerInstance = Instantiate(_languageDividerTemplate, parent);
                _languageDividerEntries.Add(dividerInstance);

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




