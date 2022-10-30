using System;
using Cysharp.Threading.Tasks;
using StarterCore.Core.Scenes.Board;
using StarterCore.Core.Scenes.Board.Card.Cards;
using StarterCore.Core.Scenes.Board.Displayer;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace StarterCore.Core.Scenes.Board.Deck
{
    public class EntityCardController : MonoBehaviour
    {
        /// <summary>
        /// Manage events from a EntityCardDisplayer
        /// Manage deck content depending on filtering event (initial / current)
        /// Manage card to be displayed depending on Slider event
        /// </summary>
        // Start is called before the first frame update

        [SerializeField] EntityCardDisplayer _cardDisplayer;

        public int currentCard;//Utilepour le Board controller qui va la passer au Challenge (-> Challenge evaluator)

        public event Action<string> OnHierarchyEntityClickedCardController;

        public void Show(EntityCard card)
        {
            _cardDisplayer.Show(card);
            //_cardDisplayer.EntityCardDispl_HierarchyClickEvent += _cardDisplayer_OnHierarchyEntityClickedCardDisplayer;
        }

        private void _cardDisplayer_OnHierarchyEntityClickedCardDisplayer(string id)
        {
            OnHierarchyEntityClickedCardController?.Invoke(id);
        }

        public void GhostBackground()
        {
            _cardDisplayer.GhostBackground();
        }

        public void ReinitBackground()
        {
            _cardDisplayer.ReinitBackground();
        }

        public void Refresh(EntityCard card)
        {
            _cardDisplayer.Refresh(card);
        }
    }
}