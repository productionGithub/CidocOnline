using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterCore.Core.Scenes.Board.Displayer;

namespace StarterCore.Core.Scenes.Board.Controller
{
    public class EntityCardsController : MonoBehaviour
    {
        // This is the level DECK DATA
        // Init + Current deck content

        //Fetch EntityDeck -> List of all entitycards' data
        //i.e. Dictionary<string, EntityCard>




        // Reference to every deck of the board
        [SerializeField] private EntityCardDisplayer _leftEntityCardDisplayer;
        [SerializeField] private EntityCardDisplayer _middleEntityCardDisplayer;


        //Events of Interactables


        public void Show()
        {
            Debug.Log("[EntityCardController] EntityCard controller initialized");
            _leftEntityCardDisplayer.Show();
            _middleEntityCardDisplayer.Show();

            //Displayer events
            _leftEntityCardDisplayer.OnSliderValueChangedDisplayer += OnLeftSliderValueChangedController;
            _middleEntityCardDisplayer.OnSliderValueChangedDisplayer += OnMiddleSliderValueChangedController;
        }

        private void OnLeftSliderValueChangedController(float value)
        {
            _leftEntityCardDisplayer.Id.text = value.ToString();
            Debug.Log("[EntityController] Deck slided is : " + _leftEntityCardDisplayer.transform.parent.name);
            Debug.Log("[EntityController] Slider value of slider is : " + value);
        }

        private void OnMiddleSliderValueChangedController(float value)
        {
            Debug.Log("[EntityController] Deck slided is : " + _middleEntityCardDisplayer.transform.parent.name);
            Debug.Log("[EntityController] Slider value of slider is : " + value);
        }
    }
}
