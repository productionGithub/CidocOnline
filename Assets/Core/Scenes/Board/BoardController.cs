#define TRACE_OFF
using UnityEngine;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Deck;
using StarterCore.Core.Services.GameState;
using System.Collections.Generic;
using Zenject;
using System.Linq;
using StarterCore.Core.Scenes.Board.Card.Cards;
using StarterCore.Core.Scenes.Board.Displayer;
using StarterCore.Core.Utils;
using Cysharp.Threading.Tasks;

using static ImageUtilities;
using UnityEngine.UI;
using System;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardController : MonoBehaviour
    {
        /// <summary>
        /// Pass board UI events to manager (Pause, Blurr panel, Validate button, Arrow animation, ...),
        /// challenge events and decks events.
        /// Has ref to a challenge controller (UI content and Evaluation).
        /// Has ref to Entity & Property services
        /// </summary>

        [Inject] NavigationService _navigationService;
        [Inject] GameStateManager _gameStateManager;
        [Inject] EntityDeckService _entityDeckService;
        [Inject] PropertyDeckService _propertyDeckService;

        public event Action OnUpdateSession;

        [SerializeField] EntityDeckController _leftEntityDeckController;
        [SerializeField] PropertyDeckController _leftPropertyDeckController;
        [SerializeField] EntityDeckController _middleEntityDeckController;
        [SerializeField] PropertyDeckController _rightPropertyDeckController;
        [SerializeField] EntityDeckController _rightEntityDeckController;

        [SerializeField] InstanceCardDisplayer _leftInstanceDisplayer;
        [SerializeField] InstanceCardDisplayer _middleInstanceDisplayer;
        [SerializeField] InstanceCardDisplayer _rightInstanceDisplayer;

        [SerializeField] Button _refreshBoard;
        [SerializeField] Button _validateBoard;
        [SerializeField] Button _nextChallenge;
        [SerializeField] Button _retryChallenge;
        [SerializeField] Button _quitChapter;

        [SerializeField] Button _mainMenu;

        [SerializeField] ChallengeController _challengeController;

        private Texture2D image;//Instance cards loaded from server

        private readonly string  _iconUrl = "https://ontomatchgame.huma-num.fr/StreamingAssets/scenarii";

        private List<ChallengeData> _challengeList;
        private List<InstanceCardModelDown> _instances;

        private ChallengeData _playerResults;
        private bool _challengeIndexUpdated;

        public void Init(List<ChallengeData> challengeList, List<InstanceCardModelDown> instances)
        {
            _refreshBoard.onClick.AddListener(Show);
            _validateBoard.onClick.AddListener(OnValidateBoard);

            _nextChallenge.onClick.AddListener(NextChallenge);
            _retryChallenge.onClick.AddListener(Show);

            _mainMenu.onClick.AddListener(BackToMainMenu);
            _challengeController.Init(challengeList);

            _quitChapter.onClick.AddListener(QuitChapter);
        }

        public void InitBoard(List<ChallengeData> challengeList, List<InstanceCardModelDown> instances)
        {
            _instances = instances;
            _challengeList = challengeList;
            _challengeIndexUpdated = false;
            _quitChapter.gameObject.SetActive(false);
        }

        public void Show()
        {
            _validateBoard.interactable = true;
            _refreshBoard.interactable = true;
            _retryChallenge.interactable = false;
            _nextChallenge.interactable = false;

            //Invalidate Retry button if last challenge
            if(_gameStateManager.GameStateModel.CurrentChallengeIndex.Equals(_challengeList.Count - 1))
            {
                _nextChallenge.gameObject.SetActive(false);
                _quitChapter.gameObject.SetActive(true);
            }

            _challengeController.Show();

            InstanciateDecks();
        }

        private async void InstanciateDecks()
        {
            _leftEntityDeckController.gameObject.SetActive(false);
            _leftPropertyDeckController.gameObject.SetActive(false);
            _middleEntityDeckController.gameObject.SetActive(false);
            _rightPropertyDeckController.gameObject.SetActive(false);
            _rightEntityDeckController.gameObject.SetActive(false);

            //Initialization of Left Entity Deck
            string initStringEL = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].ELeftInit;
            Trace.Log("ELeftInit Sctring->" + initStringEL);

            if (!initStringEL.Equals(string.Empty))
            {
                List<EntityCard> initialLeftEntityDeckContent = _entityDeckService.GetInitialDeck(initStringEL);
                Trace.Log("[BoardController] Initial entity deck size is " + initialLeftEntityDeckContent.Count);

                _leftEntityDeckController.Init(initialLeftEntityDeckContent);
                _leftEntityDeckController.InitDeck(initialLeftEntityDeckContent);

                _leftEntityDeckController.Show();
                _leftEntityDeckController.gameObject.SetActive(true);
            }
            
            //Initialization of Left Property Deck
            string initStringPL = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].PLeftInit;
            if (!initStringPL.Equals(string.Empty))
            {
                List<PropertyCard> initialLeftPropertyDeckContent = _propertyDeckService.GetInitialDeck(initStringPL);

                _leftPropertyDeckController.Init(initialLeftPropertyDeckContent);
                _leftPropertyDeckController.InitDeck(initialLeftPropertyDeckContent);
                _leftPropertyDeckController.Show();
                _leftPropertyDeckController.gameObject.SetActive(true);
            }

            //Initialization of Middle Entity Deck
            string initStringEM = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].EMiddleInit;
            if (!initStringEM.Equals(string.Empty))
            {
                List<EntityCard> initialMiddleEntityDeckContent = _entityDeckService.GetInitialDeck(initStringEM);

                _middleEntityDeckController.Init(initialMiddleEntityDeckContent);
                _middleEntityDeckController.InitDeck(initialMiddleEntityDeckContent);
                _middleEntityDeckController.Show();
                _middleEntityDeckController.gameObject.SetActive(true);
            }

            //Initialization of Right Property Deck
            string initStringPR = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].PRightInit;
            if (!initStringPR.Equals(string.Empty))
            {
                List<PropertyCard> initialRightPropertyDeckContent = _propertyDeckService.GetInitialDeck(initStringPR);

                _rightPropertyDeckController.Init(initialRightPropertyDeckContent);
                _rightPropertyDeckController.InitDeck(initialRightPropertyDeckContent);
                _rightPropertyDeckController.Show();
                _rightPropertyDeckController.gameObject.SetActive(true);
            }

            //Initialization of Right Entity Deck
            string initStringER = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].ERightInit;
            if (!initStringER.Equals(string.Empty))
            {
                List<EntityCard> initialRightEntityDeckContent = _entityDeckService.GetInitialDeck(initStringER);

                _rightEntityDeckController.Init(initialRightEntityDeckContent);
                _rightEntityDeckController.InitDeck(initialRightEntityDeckContent);
                _rightEntityDeckController.Show();
                _rightEntityDeckController.gameObject.SetActive(true);
            }

            //INSTANCES
            _leftInstanceDisplayer.gameObject.SetActive(false);
            _middleInstanceDisplayer.gameObject.SetActive(false);
            _rightInstanceDisplayer.gameObject.SetActive(false);

            //Left instance
            string initStringLI = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].ILeftInit;
            if (!initStringLI.Equals(string.Empty))
            {
                InstanceCardModelDown leftInstance = _instances.Single(c => c.Id == initStringLI[1..].Trim());

                string iconName = leftInstance.ImageName;
                string url = _iconUrl + "/" + _gameStateManager.GameStateModel.CurrentScenario + "/Instances/Images/" + iconName;

                image = await DownloadJPGImage(url, "leftEntity");

                Sprite iconSprite = Sprite.Create(image, new Rect(0, 0, 300, 300), new Vector2());

                _leftInstanceDisplayer.Show(leftInstance, iconSprite);
                _leftInstanceDisplayer.gameObject.SetActive(true);
            }

            ////Middle instance
            string initStringMI = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].IMiddleInit;
            if (!initStringMI.Equals(string.Empty))
            {
                InstanceCardModelDown middleInstance = _instances.Single(c => c.Id == initStringMI[1..].Trim());

                string iconName = middleInstance.ImageName;
                string url = _iconUrl + "/" + _gameStateManager.GameStateModel.CurrentScenario + "/Instances/Images/" + iconName;

                image = await DownloadJPGImage(url, "middleEntity");

                Sprite iconSprite = Sprite.Create(image, new Rect(0, 0, 300, 300), new Vector2());
                _middleInstanceDisplayer.Show(middleInstance, iconSprite);
                _middleInstanceDisplayer.gameObject.SetActive(true);
            }

            ////Right instance
            string initStringRI = _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].IRightInit;
            if (!initStringRI.Equals(string.Empty))
            {
                InstanceCardModelDown rightInstance = _instances.Single(c => c.Id == initStringRI[1..].Trim());

                string iconName = rightInstance.ImageName;
                string url = _iconUrl + "/" + _gameStateManager.GameStateModel.CurrentScenario + "/Instances/Images/" + iconName;

                image = await DownloadJPGImage(url, "RightEntity");

                Sprite iconSprite = Sprite.Create(image, new Rect(0, 0, 300, 300), new Vector2());
                _rightInstanceDisplayer.Show(rightInstance, iconSprite);
                _rightInstanceDisplayer.gameObject.SetActive(true);
            }

        }

        private void OnValidateBoard()
        {
            //Update ChallengeData answers fields.
            //Player's answers are stored in a copy of the challenges[currentChallengeId] 'ChallengeData' object
            //'__Anwsers' properties of this copy are update with players answers
            // This copy is then compared with expected results in the original ChallengeData object when the Validate button is pressed.
            ChallengeData playerResults = new ChallengeData();

            if(_leftEntityDeckController.isActiveAndEnabled)
            {
                playerResults.ELeftAnswer = _leftEntityDeckController.CurrentCard.id;
            }
            if (_leftPropertyDeckController.isActiveAndEnabled)
            {
                playerResults.PLeftAnswer = _leftPropertyDeckController.CurrentCard.id;
            }
            if(_middleEntityDeckController.isActiveAndEnabled)
            {
                playerResults.EMiddleAnswer = _middleEntityDeckController.CurrentCard.id;
            }
            if(_rightPropertyDeckController.isActiveAndEnabled)
            {
                playerResults.PRightAnswer = _rightPropertyDeckController.CurrentCard.id;
            }
            if(_rightEntityDeckController.isActiveAndEnabled)
            {
            playerResults.ERightAnswer = _rightEntityDeckController.CurrentCard.id;
            }

            //Evaluate board
            bool isCorrect = _challengeController.EvaluateBoard(_challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex], playerResults);
            if(isCorrect)
            {
                Trace.Log("[BoardController] ANSWER CORRECT!");
                //update button states
                _validateBoard.interactable = false;
                _refreshBoard.interactable = false;
                _retryChallenge.interactable = false;
                _nextChallenge.interactable = true;

                //update game model
                _gameStateManager.GameStateModel.CurrentScore += _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex].Score;
                if(_gameStateManager.GameStateModel.CurrentChallengeIndex < _challengeList.Count - 1)
                {
                    _gameStateManager.GameStateModel.CurrentChallengeIndex++;
                }

                _challengeIndexUpdated = true;

                //update DB
                OnUpdateSession?.Invoke();
            }
            else
            {
                Trace.Log("[BoardController] ANSWER NOT CORRECT!");
                _validateBoard.interactable = false;
                _refreshBoard.interactable = false;
                _retryChallenge.interactable = true;
                _nextChallenge.interactable = true;
            }

            _challengeController.DisplayResult(isCorrect, _challengeList[_gameStateManager.GameStateModel.CurrentChallengeIndex]);
        }

        private void NextChallenge()
        {
            //If answer was right, challengeIndex already incremented, just reset the flag
            if(_challengeIndexUpdated)
            {
                _challengeIndexUpdated = false;
            }
            else
            {
                //If answer was wrong, challengeIndex was not incremented.
                _gameStateManager.GameStateModel.CurrentChallengeIndex++;
                OnUpdateSession?.Invoke();
            }

            Show();
            Trace.Log("CurrentChallengeIndex is : " + _gameStateManager.GameStateModel.CurrentChallengeIndex);
        }

        private void QuitChapter()
        {
            //If player quits before validating, we need to update the session anyway
            if (_challengeIndexUpdated)
            {
                _challengeIndexUpdated = false;
            }
            else
            {
                //If answer was wrong, challengeIndex was not incremented.
                if (_gameStateManager.GameStateModel.CurrentChallengeIndex < _challengeList.Count - 1)
                {
                    _gameStateManager.GameStateModel.CurrentChallengeIndex++;
                }
                OnUpdateSession?.Invoke();
            }
            _navigationService.Push("MainMenuScene");
        }

        public async UniTask<Texture2D> DownloadJPGImage(string url, string name)
        {
            Texture2D img = await ImageDownloader.DownloadImage(url, name, FileFormat.JPG);
            return img;
        }

        private void BackToMainMenu()
        {
            _navigationService.Clear("MainMenuScene");
        }

        private void OnDestroy()
        {
            _refreshBoard.onClick.RemoveListener(Show);
            _validateBoard.onClick.RemoveListener(OnValidateBoard);
            _retryChallenge.onClick.RemoveListener(Show);
            _nextChallenge.onClick.RemoveListener(Show);
            _mainMenu.onClick.RemoveListener(BackToMainMenu);
            _quitChapter.onClick.RemoveListener(QuitChapter);
        }
    }
}