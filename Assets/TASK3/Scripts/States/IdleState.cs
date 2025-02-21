using AxGrid;
using AxGrid.FSM;
using UnityEngine;

namespace LootboxFSM
{
    [State("Idle")]
    public class IdleState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Log.Debug($"{Parent.CurrentStateName} ENTER");
            Settings.Model.Set("CanStart", true);
            Settings.Model.Set("CanStop", false);
            Settings.Model.EventManager.Invoke("CanStartChanged");
            Settings.Model.EventManager.Invoke("CanStopChanged");
        }

        [Exit]
        private void ExitThis()
        {
            Log.Debug($"{Parent.CurrentStateName} EXIT");
        }
    }
}
