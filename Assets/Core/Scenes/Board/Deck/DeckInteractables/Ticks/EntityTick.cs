#define Trace_OFF
using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables.Ticks
{
    public class EntityTick : MonoBehaviour
    {
        public enum TickColor
        {
            White,
            Blue,
            Brown,
            Yellow,
            Pink,
            Green,
            BabyBlue,
            Grey,
            Purple
        }

        public event Action<GameObject, TickColor> OnEntityTickClicked;

        public bool IsTicked { get => _ticked; }
        public TickColor ColorOfTick { get => _tickColor; }

        //Fields
        [SerializeField]
        private GameObject _tick;//Attached Tick object
        [SerializeField]
        private TickColor _tickColor;//Color of the tick

        //Private members
        private readonly float _translateValue = 12.0f;//Ou -12 ? à tester
        private bool _ticked = false;//Initial state is off

        public void Init()
        {
            Trace.Log("[EntityTicks] Ticks eventlistener added !");
            if (GetComponent<Button>() != null)
            {
                GetComponent<Button>().onClick.AddListener(Clicked);
            }
            else
            {
                throw new ArgumentNullException("EntityTick component must be attached to a button.");
            }
        }

        private void Clicked()
        {
            Trace.Log("[EntityTicks] Tick clicked !");
            OnEntityTickClicked?.Invoke(_tick, _tickColor);
        }

        //UI of tick, set it 'Off' or 'On'
        public void TickOn()
        {
            Trace.Log("[EntityTicks] Tick is ON !");
            if (IsTicked == false)
            {
                transform.parent.Translate(0.0f, -_translateValue, 0.0f);
                _ticked = true;
            }
        }

        //UI of tick, set it 'Off' or 
        public void TickOff()
        {
            Debug.Log("[EntityTicks] Tick is OFF !");
            if (IsTicked == true)
            {
                transform.parent.Translate(0.0f, _translateValue, 0.0f);
                _ticked = false;
            }
        }
    }
}
