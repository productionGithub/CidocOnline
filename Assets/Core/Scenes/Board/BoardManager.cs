using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardManager : IInitializable
    {
        /// <summary>
        /// LOGIQUE METIER :
        /// INJECT LE COMPOSANT QUI EVALUE LE PLATEAU
        /// INJECTE LE COMPOSANT QUI UPDATE LE MODELE
        /// etc...
        /// </summary>
        ///

        [Inject] CardController cardCtrl;

        public void Initialize()
        {
            cardCtrl.Show();
            cardCtrl.OnCardBubbleUp += (CardDisplayer c, TickDisplayer t) => DoSomethingWithIt(c, t);
        }

        private void DoSomethingWithIt(CardDisplayer c, TickDisplayer t)
        {
            Debug.Log(string.Format("[Board Manager] Evaluate Board with {0} and {1}", c.name, t.name));
        }
    }
}
