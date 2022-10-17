using Zenject;

namespace StarterCore.Core.Scenes.Board
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class DeckManager// : IInitializable
    {
        /*
        [Inject] private CardDisplayer _LeftDeckTickcontroller;

        
        public void Initialize()
        {
            _LeftDeckTickcontroller.Show();
            _LeftDeckTickcontroller.OnCardClicked += _LeftController_OnTickSet;

            _LeftDeckTickcontroller.Show();
        }


        //Little workaround to debug.log the name of the the ticker that has been clicked
        private void _LeftController_OnTickSet(TickDisplayer tick)
        {
            if (tick.isOn)
            {
                string s = string.Format("LEFT DECK - Deck will SET filter on {0}", tick.name);
                _LeftDeckTickcontroller.DisplayMsg(s);
            }
            else
            {
                string s = string.Format("LEFT DECK - will UNSET filter on {0}", tick.name);
                _LeftDeckTickcontroller.DisplayMsg(string.Format(s));

            }

        }
        */

    }
}