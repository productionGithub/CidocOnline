using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterCore.Core.Scenes.Board.Displayer;
using StarterCore.Core.Scenes.Board.Challenge;
using StarterCore.Core.Scenes.Board.Card.Cards;
using Zenject;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.Board.Controller
{
    public class EntityCardsController : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;

        // This is the level DECK DATA
        // Init + Current deck content

        //Fetch EntityDeck -> List of all entitycards' data
        //i.e. Dictionary<string, EntityCard>




        // Reference to every deck of the board
        [SerializeField] private EntityCardController _leftEntityCardController;
        //[SerializeField] private EntityCardController _middleEntityCardController;


        //Events of Interactables

        public void Show()
        {
            Debug.Log("[EntityCardController] EntityCard controller initialized");

            Debug.Log(string.Format("[EntityCardController] Init cEntityCards with challenge data {0} / {1} / {2}",
                _gameStateManager.GameStateModel.CurrentScenario, _gameStateManager.GameStateModel.CurrentChapter, _gameStateManager.GameStateModel.CurrentChallengeIndex));


            //_leftEntityCardController.Show();
            //_middleEntityCardController.Show();

            //_leftEntityCardController.OnSliderValueChangeController += OnLeftSliderValueChangedController;
            ////_middleEntityCardController.OnSliderValueChangeController += OnMiddleSliderValueChangedController;
        }

        private void OnLeftSliderValueChangedController(float value)
        {
            //_leftEntityCardController.Refresh(new EntityCard());
        }

        private void OnMiddleSliderValueChangedController(float value)
        {
        }
    }
}
