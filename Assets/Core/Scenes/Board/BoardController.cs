#define TRACE_ON
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
using StarterCore.Core.Services.Network;
using StarterCore.Core.Utils;
using System.Collections;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

using static ImageUtilities;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardController : MonoBehaviour
    {
        /// <summary>
        /// Pass board UI events to manager (Pause, Blurr panel, Validate button, Arrow animation, ...),
        /// challenge events and decks events.
        /// Has ref to a challenge controller (UI content and Evaluation).
        /// Has ref to Entity, Property and Instance cards.
        /// </summary>

        [Inject] MockNetService _networkService;
        [Inject] NavigationService _navigationService;
        [Inject] GameStateManager _gameStateManager;
        [Inject] EntityDeckService _entityDeckService;
        [Inject] PropertyDeckService _propertyDeckService;

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

        [SerializeField] ChallengeController _challengeController;

        private Texture2D image;//Instance cards loaded from server

        private readonly string  _iconUrl = "https://ontomatchgame.huma-num.fr/StreamingAssets/scenarii";

        private List<ChallengeData> _challengeList;
        private List<InstanceCardModelDown> _instances;

        private bool _leftEntityDeckControllerBool = false;

        public async void Show(List<ChallengeData> challengeList)
        {
            _challengeList = challengeList;
            _instances = await _networkService.GetInstanceFile(_gameStateManager.GameStateModel.CurrentScenario);

            _refreshBoard.onClick.AddListener(OnRefreshBoard);
            _validateBoard.onClick.AddListener(OnValidateBoard);

            _challengeController.Show(_challengeList);

            InstanciateDecks();
        }

        private async void InstanciateDecks()
        {

            int currentChallengeId = _gameStateManager.GameStateModel.CurrentChallengeIndex;

            //Initialization of Left Entity Deck
            string initStringEL = _challengeList[currentChallengeId].ELeftInit;
            if (!initStringEL.Equals(string.Empty))
            {
                _leftEntityDeckController.gameObject.SetActive(true);
                List<EntityCard> initialLeftEntityDeckContent = _entityDeckService.GetInitialDeck(initStringEL);
                Trace.Log("[BoardCOntroller] Call EntityDeckController Init()!");
                if (_leftEntityDeckControllerBool == false)//A refactor
                {
                    _leftEntityDeckController.Init();
                    _leftEntityDeckControllerBool = true;
                }
                _leftEntityDeckController.Show(initialLeftEntityDeckContent);
            }

            /*
            //Initialization of Left Property Deck
            string initStringPL = _challengeList[currentChallengeId].PLeftInit;
            if (!initStringPL.Equals(string.Empty))
            {
                _leftPropertyDeckController.gameObject.SetActive(true);
                List<PropertyCard> initialLeftPropertyDeckContent = _propertyDeckService.GetInitialDeck(initStringPL);
                _leftPropertyDeckController.Show(initialLeftPropertyDeckContent);
            }

            //Initialization of Middle Entity Deck
            string initStringEM = _challengeList[currentChallengeId].EMiddleInit;
            if (!initStringEM.Equals(string.Empty))
            {
                _middleEntityDeckController.gameObject.SetActive(true);
                List<EntityCard> initialMiddleEntityDeckContent = _entityDeckService.GetInitialDeck(initStringEM);
                _middleEntityDeckController.Show(initialMiddleEntityDeckContent);
            }

            //Initialization of Right Property Deck
            string initStringPR = _challengeList[currentChallengeId].PRightInit;
            if (!initStringPR.Equals(string.Empty))
            {
                _rightPropertyDeckController.gameObject.SetActive(true);
                List<PropertyCard> initialRightPropertyDeckContent = _propertyDeckService.GetInitialDeck(initStringPR);
                _rightPropertyDeckController.Show(initialRightPropertyDeckContent);
            }

            //Initialization of Right Entity Deck
            string initStringER = _challengeList[currentChallengeId].ERightInit;
            if (!initStringER.Equals(string.Empty))
            {
                _rightEntityDeckController.gameObject.SetActive(true);
                List<EntityCard> initialRightEntityDeckContent = _entityDeckService.GetInitialDeck(initStringER);
                _rightEntityDeckController.Show(initialRightEntityDeckContent);
            }

            */
            //INSTANCES
            //Left instance
            string initStringLI = _challengeList[currentChallengeId].ILeftInit;
            if (!initStringLI.Equals(string.Empty))
            {
                _leftInstanceDisplayer.gameObject.SetActive(true);

                //<--- THIS BLOC GOES TO THE DISPLAYER
                InstanceCardModelDown leftInstance = _instances.Single(c => c.Id == initStringLI[1..].Trim());

                string iconName = leftInstance.ImageName;
                string url = _iconUrl + "/" + _gameStateManager.GameStateModel.CurrentScenario + "/Instances/Images/" + iconName;

                image = await DownloadJPGImage(url, "leftEntity");

                Sprite iconSprite = Sprite.Create(image, new Rect(0, 0, 300, 300), new Vector2());
                //--->

                _leftInstanceDisplayer.Show(leftInstance, iconSprite);
            }
            /*
            ////Middle instance
            string initStringMI = _challengeList[currentChallengeId].IMiddleInit;
            if (!initStringMI.Equals(string.Empty))
            {
                _middleInstanceDisplayer.gameObject.SetActive(true);
                InstanceCardModelDown middleInstance = _instances.Single(c => c.Id == initStringMI[1..].Trim());

                string iconName = middleInstance.ImageName;
                string url = _iconUrl + "/" + _gameStateManager.GameStateModel.CurrentScenario + "/Instances/Images/" + iconName;

                image = await DownloadJPGImage(url, "middleEntity");

                Sprite iconSprite = Sprite.Create(image, new Rect(0, 0, 300, 300), new Vector2());
                _middleInstanceDisplayer.Show(middleInstance, iconSprite);
            }

            ////Right instance
            string initStringRI = _challengeList[currentChallengeId].IRightInit;
            if (!initStringRI.Equals(string.Empty))
            {
                _rightInstanceDisplayer.gameObject.SetActive(true);
                InstanceCardModelDown rightInstance = _instances.Single(c => c.Id == initStringRI[1..].Trim());

                string iconName = rightInstance.ImageName;
                string url = _iconUrl + "/" + _gameStateManager.GameStateModel.CurrentScenario + "/Instances/Images/" + iconName;

                image = await DownloadJPGImage(url, "RightEntity");

                Sprite iconSprite = Sprite.Create(image, new Rect(0, 0, 300, 300), new Vector2());
                _rightInstanceDisplayer.Show(rightInstance, iconSprite);
            }
            */
        }

        private void OnValidateBoard()
        {
            if(_gameStateManager.GameStateModel.CurrentChallengeIndex < _challengeList.Count - 1)//Challenge 0 does not count
            {
                //Evaluate
                //Display ad-hoc message
                //Animate icon continue
                //Wait for click on continue or retry
                _gameStateManager.GameStateModel.CurrentChallengeIndex++;
                InstanciateDecks();
            }
            else
            {
                Debug.Log("Last challenge played, go to stats... or somethign else");
            }
        }

        private void OnRefreshBoard()
        {
            if (_leftEntityDeckController.gameObject.activeSelf)
            {
                _leftEntityDeckController.ResetDeck();
            }
            if (_middleEntityDeckController.gameObject.activeSelf)
            {
                _middleEntityDeckController.ResetDeck();
            }
            if (_rightEntityDeckController.gameObject.activeSelf)
            {
                _rightEntityDeckController.ResetDeck();
            }
            if (_leftPropertyDeckController.gameObject.activeSelf)
            {
                _leftPropertyDeckController.ResetDeck();
            }
            if (_rightPropertyDeckController.gameObject.activeSelf)
            {
                _rightPropertyDeckController.ResetDeck();
            }
        }

        private void OnGamePaused()
        {
        }

        private void OnAnimateArrow()
        {
        }

        private void OntickClicked()
        {
        }

        private void OnDestroy()
        {
        }

        public async UniTask<Texture2D> DownloadJPGImage(string url, string name)
        {
            Texture2D img = await ImageDownloader.DownloadImage(url, name, FileFormat.JPG);
            return img;
        }
    }
}