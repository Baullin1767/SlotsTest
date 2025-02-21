using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace LootboxFSM
{
    [State("Stopping")]
    public class StoppingState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");
            Settings.Model.Set("CanStop", false);

            var scroller = Settings.Model.Get<SlotScroller>("SlotScroller0");
            scroller?.StopSpinning();
            scroller = Settings.Model.Get<SlotScroller>("SlotScroller1");
            scroller?.StopSpinning();
            scroller = Settings.Model.Get<SlotScroller>("SlotScroller2");
            scroller?.StopSpinning();
        }

        [One(6f)]
        private void ChangeState()
        {
            Parent.Change("Idle");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}
