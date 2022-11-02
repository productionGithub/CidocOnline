#define TRACE_ON
using System;
using System.Collections.Generic;
using StarterCore.Core.Scenes.Board.Card.Cards;
using StarterCore.Core.Scenes.Board.Displayer;
using UnityEngine;
using Zenject;
using StarterCore.Core.Scenes.Board.Deck;
using System.Linq;

namespace StarterCore.Core.Scenes.Board.Deck
{
    public class EntityCardController : MonoBehaviour
    {
        /// <summary>
        /// Manage events from a EntityCardDisplayer (FullText button event for scope note)
        /// Proxy methods to entity card displayer
        /// </summary>
        
        [SerializeField]
        EntityCardDisplayer _entityCardDisplayer;


        public int currentCard;//Utilepour le Board controller qui va la passer au Challenge (-> Challenge evaluator)

        public event Action<string> OnHierarchyEntityClickedCardController;

        public void Init()
        {
            _entityCardDisplayer.Init();
        }

        public void Show(EntityCard card)
        {
            _entityCardDisplayer.Show(card);
            //_cardDisplayer.EntityCardDispl_HierarchyClickEvent += _cardDisplayer_OnHierarchyEntityClickedCardDisplayer;
        }

        private void _cardDisplayer_OnHierarchyEntityClickedCardDisplayer(string id)
        {
            OnHierarchyEntityClickedCardController?.Invoke(id);
        }

        public void GhostBackground()
        {
            _entityCardDisplayer.GhostBackground();
        }

        public void ReinitBackground()
        {
            _entityCardDisplayer.ReinitBackground();
        }

        public void Refresh(EntityCard card)
        {
            _entityCardDisplayer.Show(card);
        }

        private void OnDestroy()
        {
            //Unsubscribe hierarchy event
        }
    }
}