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
    public class PropertyCardController : MonoBehaviour
    {
        [SerializeField]
        PropertyCardDisplayer _propertyCardDisplayer;

        public event Action<string> OnDomainButtonClick_Controller;
        public event Action<string> OnRangeButtonClick_Controller;


        public int currentCard;//???

        public event Action<string> OnHierarchyPropertyClickedCardController;

        public void Init()
        {
            _propertyCardDisplayer.Init();
            _propertyCardDisplayer.OnDomainButtonClick_Displayer += DomainButtonClicked;
            _propertyCardDisplayer.OnRangeButtonClick_Displayer += RangeButtonClicked;
        }

        private void DomainButtonClicked(string about)
        {
            OnDomainButtonClick_Controller?.Invoke(about);
        }

        private void RangeButtonClicked(string about)
        {
            OnRangeButtonClick_Controller?.Invoke(about);
        }

        public void Show(PropertyCard card)
        {
            _propertyCardDisplayer.Show(card);
        }

        public void GhostBackground()
        {
            _propertyCardDisplayer.GhostBackground();
        }

        public void ReinitBackground()
        {
            _propertyCardDisplayer.ReinitBackground();
        }

        public void Refresh(PropertyCard card)
        {
            _propertyCardDisplayer.Show(card);
        }

        private void OnDestroy()
        {
            //Unsubscribe hierarchy event
        }
    }
}