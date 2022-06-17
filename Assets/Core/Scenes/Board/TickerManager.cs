using Zenject;

namespace StarterCore.Core.Scenes.Board
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class TickerManager : IInitializable
    {

        [Inject] private TickerController _controller;
        
        public void Initialize()
        {
            _controller.Show();
            _controller.OnTickSet += _controller_OnTickSet;
        }


        //Little workaround to debug.log the name of the the ticker that has been clicked
        private void _controller_OnTickSet(Ticker tick)
        {
            _controller.DisplayMsg(tick.name);
            if (tick.isOn)
            {
                string s = string.Format("Deck will SET filter on {0}", tick.name);
                _controller.DisplayMsg(s);
            }
            else
            {
                string s = string.Format("Deck will UNSET filter on {0}", tick.name);
                _controller.DisplayMsg(string.Format(s));

            }

        }
    }
}