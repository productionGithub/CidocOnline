#define TRACE_OFF
using System;
using UnityEngine;

namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables.Ticks
{
    public class EntityTicksController : MonoBehaviour
    {
        public event Action<GameObject, EntityTick.TickColor> OnEntityTickClicked_TicksCtrl;

        public EntityTick whiteTick;

        private int _tickCount; //Track number of tick set to On. If 0, set White tick to On
        public int TickCount { get => _tickCount; }

        private EntityTick[] listOfTick;

        public void Init()
        {
            Trace.Log("[EntityTicksController] Init!");
            ResetTicks();//Set all ticks to off but the white one

            //Add listeners to all ticks
            listOfTick = transform.GetComponentsInChildren<EntityTick>();
            foreach (EntityTick tick in listOfTick)
            {
                tick.Init();
                tick.OnEntityTickClicked += TickClicked;
            }
        }

        public void TickClicked(GameObject sender, EntityTick.TickColor color)
        {
            Trace.Log("[EntityTicksController] Tick clicked !");
            RefreshTicks(sender);
            OnEntityTickClicked_TicksCtrl?.Invoke(sender, color);
        }

        // RefreshTicks manages the UI of Ticks:
        // - If White is selected, all others are OFF
        // - If at least one but the White is selected: White is OFF
        // - If all but White are Off, White is set to On
        public void RefreshTicks(GameObject senderTick)
        {
            EntityTick tick = senderTick.GetComponentInParent<EntityTick>();

            if (tick != null)
            {
                if (tick.ColorOfTick == EntityTick.TickColor.White)
                {
                    ResetTicks();//White On, all others Off
                    _tickCount = 0;
                }
                else
                {
                    if (tick.IsTicked)
                    {
                        tick.TickOff();
                        _tickCount--;
                        if (_tickCount == 0)
                        {
                            whiteTick.GetComponent<EntityTick>().TickOn();
                        }
                    }
                    else
                    {
                        tick.TickOn();
                        _tickCount++;
                        whiteTick.GetComponent<EntityTick>().TickOff();
                    }
                }
            }
        }

        public void ResetTicks()
        {
            //Set all ticks to Off
            foreach (EntityTick tick in GetComponentsInChildren<EntityTick>())
            {
                tick.TickOff();
            }
            //Set white tick to On
            whiteTick.GetComponent<EntityTick>().TickOn();
            _tickCount = 0;
        }

        private void OnDestroy()
        {
            foreach (EntityTick tick in listOfTick)
            {
                tick.OnEntityTickClicked -= TickClicked;
            }
        }
    }
}
