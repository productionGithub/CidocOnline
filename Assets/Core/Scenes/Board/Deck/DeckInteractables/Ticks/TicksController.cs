#define TRACE_OFF
using UnityEngine;

/// <summary>
/// Acts as a proxy:
/// Calls Entity or Property 'UpdateColorFilter()' depending on type of deck
/// attached to the caller tick.
/// </summary>
///
namespace StarterCore.Core.Scenes.Board.Deck
{
    public class TicksController : MonoBehaviour
    {
        public TickCtrl whiteTick;

        [SerializeField]
        private GameObject deckContainer;

        //Track number of tick set to On
        //If 0, set White tick to On
        private int tickCount;
        public int TickCount { get => tickCount; }

        void Start()
        {
            //Set White tick to ON (all others by default are OFF)
            whiteTick.GetComponent<TickCtrl>().TickOn();
        }

        //Call for color filtering depending on type of decks.
        public void TickClicked(GameObject sender, TickCtrl.TickColor e)
        {
            Trace.Log("TICKS CONTROLLER - TICK CLICKED !");
            //Update tick state and UI
            RefreshTicks(sender);

            //Calls ad-hoc filtering method depending on type of deck managed by Ticks
            if (deckContainer.GetComponent<EntityDeckController>() != null)
            {
                Trace.Log("TICKS CONTROLLER - TICK CLICKED IS FROM ENTITY CARD!");
                //deckContainer.GetComponent<EntityDeckController>().UpdateColorFilters(sender, e);
            }
            else
            {
                if (deckContainer.GetComponent<PropertyDeckController>() != null)
                {
                    Trace.Log("TICKS CONTROLLER - TICK CLICKED IS FROM PROPERTY CARD!");
                    //deckContainer.GetComponent<PropertyDeckController>().UpdateColorFilters(sender, e);
                }
            }
        }



        // RefreshTicks manages the UI of Ticks:
        // - If White is selected, all others are OFF
        // - If at least one but the White is selected: White is OFF
        // - If all but White are Off, White is set to On
        public void RefreshTicks(GameObject senderTick)
        {
            TickCtrl tick = senderTick.GetComponentInParent<TickCtrl>();

            if (tick != null)
            {
                if (tick.colorOfTick == TickCtrl.TickColor.White)
                {
                    ResetTicks();//White On, all others Off
                    tickCount = 0;
                }
                else
                {
                    if (tick.IsTicked)
                    {
                        tick.TickOff();
                        tickCount--;
                        if (tickCount == 0)
                        {
                            whiteTick.GetComponent<TickCtrl>().TickOn();
                        }
                    }
                    else
                    {
                        tick.TickOn();
                        tickCount++;
                        whiteTick.GetComponent<TickCtrl>().TickOff();
                    }
                }
            }
        }



        public void ResetTicks()
        {
            //Set all ticks to Off
            foreach (TickCtrl tick in GetComponentsInChildren<TickCtrl>())
            {
                tick.TickOff();
            }
            //Set white tick to On
            whiteTick.GetComponent<TickCtrl>().TickOn();
        }
    }
}