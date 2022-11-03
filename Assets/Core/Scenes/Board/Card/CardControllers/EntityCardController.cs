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


        public int CurrentCard;//Utile pour le Board controller qui va la passer au Challenge (-> Challenge evaluator ???

        public void Init()
        {
            _entityCardDisplayer.Init();
        }

        public void Show(EntityCard card)
        {
            _entityCardDisplayer.Show(card);
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
    }
}