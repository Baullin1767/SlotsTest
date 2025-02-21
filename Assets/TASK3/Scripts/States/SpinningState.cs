using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace LootboxFSM
{
    [State("Spinning")]
    public class SpinningState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");
            Settings.Model.Set("CanStart", false);
            Settings.Model.Set("CanStop", false);
            Settings.Model.EventManager.Invoke("CanStartChanged");
            Settings.Model.EventManager.Invoke("CanStopChanged");

            var scroller = Settings.Model.Get<SlotScroller>("SlotScroller0");
            scroller?.StartSpinning();
            scroller = Settings.Model.Get<SlotScroller>("SlotScroller1");
            scroller?.StartSpinning();
            scroller = Settings.Model.Get<SlotScroller>("SlotScroller2");
            scroller?.StartSpinning();
        }

        [One(3f)]
        private void EnableStopButton()
        {
            Settings.Model.Set("CanStop", true);
            Settings.Model.EventManager.Invoke("CanStopChanged");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}
