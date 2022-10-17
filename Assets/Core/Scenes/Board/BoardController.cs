using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Controller;
using System.Linq;

namespace StarterCore.Core.Scenes.Board
{
    public class BoardController : MonoBehaviour
    {
        //[Header("Static")]
        [SerializeField] private EntityCardsController _entityCardsController;

        Chapter bundle;

        public void Show()
        {

            Debug.Log("[Board controller] Board controller initialized");
            _entityCardsController.Show();

        }

        //private void OnFilterDomainPanel(List<string> domains)
        //{
        //    _domainCriterias = domains;
        //    Debug.Log("");
        //    FilterScenarii();
        //}


        private void OnDestroy()
        {
        }
    }
}