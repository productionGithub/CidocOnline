#define TRACE_ON
using System;
using UnityEngine;

/// <summary>
/// Acts as a proxy:
/// Calls Entity or Property 'UpdateColorFilter()' depending on type of deck
/// attached to the caller tick.
/// </summary>
///
namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables.Ticks
{
    public class PropertyTicksController : MonoBehaviour
    {
        public event Action<GameObject, PropertyTick.TickColor> OnPropertyTickClicked_TicksCtrl;

        public PropertyTick WhiteTick;
        public int TickCount { get => _tickCount; }

        //Track number of tick set to On
        //If 0, set White tick to On
        private int _tickCount;
        private PropertyTick[] listOfTick;

        private bool _initDone = false;

        public void Init()
        {
            //Initialization is made only once
            if (_initDone == false)
            {
                Trace.Log("[PropertyTicksController] Init!");

                //Add listeners to all ticks
                listOfTick = transform.GetComponentsInChildren<PropertyTick>();
                foreach (PropertyTick tick in listOfTick)
                {
                    tick.Init();
                    tick.OnPropertyTickClicked += TickClicked;
                }
                //Set White tick to ON (all others by default are OFF)
                WhiteTick.GetComponent<PropertyTick>().TickOn();

                _initDone = true;
            }
        }

        //Call for color filtering depending on type of decks.
        public void TickClicked(GameObject sender, PropertyTick.TickColor color)
        {
            //Update tick UI
            RefreshTicks(sender);
            //Notify Deckcontroller
            OnPropertyTickClicked_TicksCtrl?.Invoke(sender, color);
        }

        // RefreshTicks manages the UI of Ticks:
        // - If White is selected, all others are OFF
        // - If at least one but the White is selected: White is OFF
        // - If all but White are Off, White is set to On
        public void RefreshTicks(GameObject senderTick)
        {
            PropertyTick tick = senderTick.GetComponentInParent<PropertyTick>();

            if (tick != null)
            {
                if (tick.ColorOfTick == PropertyTick.TickColor.White)
                {
                    ResetTicks();//White On, all others Off
                }
                else
                {
                    if (tick.IsTicked)
                    {
                        tick.TickOff();
                        _tickCount--;
                        if (_tickCount == 0)
                        {
                            ResetTicks();
                            //WhiteTick.GetComponent<PropertyTick>().TickOn();
                        }
                    }
                    else
                    {
                        tick.TickOn();
                        _tickCount++;
                        WhiteTick.GetComponent<PropertyTick>().TickOff();
                    }
                }
            }
        }

        public void ResetTicks()
        {
            //Set all ticks to Off
            foreach (PropertyTick tick in GetComponentsInChildren<PropertyTick>())
            {
                tick.TickOff();
            }
            //Set white tick to On
            WhiteTick.GetComponent<PropertyTick>().TickOn();
            _tickCount = 0;
        }

        private void OnDestroy()
        {
            listOfTick = transform.GetComponentsInChildren<PropertyTick>();
            foreach (PropertyTick tick in listOfTick)
            {
                Trace.Log(string.Format("[PropertyTicksController] Unsubscribe for ", tick.ColorOfTick));
                tick.Init();
                tick.OnPropertyTickClicked -= TickClicked;
            }
        }
    }
}