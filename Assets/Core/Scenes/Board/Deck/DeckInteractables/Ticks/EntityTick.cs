#define Trace_ON
using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables.Ticks
{
    public class EntityTick : MonoBehaviour
    {
        //Public members
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
            OnEntityTickClicked?.Invoke(_tick, _tickColor);
        }

        //UI of tick, set it 'Off' or 'On'
        public void TickOn()
        {
            if (IsTicked == false)
            {
                transform.parent.Translate(0.0f, -_translateValue, 0.0f);
                _ticked = true;
            }
        }

        //UI of tick, set it 'Off' or 
        public void TickOff()
        {
            if (IsTicked == true)
            {
                transform.parent.Translate(0.0f, _translateValue, 0.0f);
                _ticked = false;
            }
        }
    }
}
