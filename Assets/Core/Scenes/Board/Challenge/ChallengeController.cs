using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Scenes.Board.Controller;
using StarterCore.Core.Scenes.Board.Challenge;

namespace StarterCore.Core.Scenes.Board
{
    public class ChallengeController : MonoBehaviour
    {
        /// <summary>
        /// Challenge controller manages:
        /// Events from displayers (cards, challenge text)
        /// Via ref to card displayers and challenge displayer
        /// Has ref to ChallengeController (Ref to all decks, all data from challenge / Evaluation)
        /// Manage / bubble up events from displayers
        /// 
        /// </summary>
        //Has ref to Entity, Property and Instance cards
        //Has ref to a challenge displayer (UI content and Evaluation)

        [Inject] NavigationService _netService;

        //[Header("Static")]



        public void Show()
        {
            Debug.Log("[ChallengeController] Init OK");
            //Load challenge data


            //challengeData.Show();//Statement
        }

    }
}