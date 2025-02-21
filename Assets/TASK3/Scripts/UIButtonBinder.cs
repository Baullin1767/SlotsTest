using AxGrid;
using AxGrid.Base;
using AxGrid.Tools.Binders;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace LootboxFSM
{
    public class UIButtonBinder : MonoBehaviourExt
    {
        public UIButtonDataBind startButton;
        public UIButtonDataBind stopButton;

        [OnStart]
        private void Init()
        {
            Model.EventManager.AddAction($"OnStartButtonClick", StartButtonAction);
            Model.EventManager.AddAction($"OnStopButtonClick", StopButtonAction);

            Settings.Model.EventManager.AddAction("CanStartChanged", UpdateStartButton);
            Settings.Model.EventManager.AddAction("CanStopChanged", UpdateStopButton);
        }

        private void StartButtonAction()
        {
            Model.EventManager.Invoke("StartSpin");
            UpdateStartButton();
        }
        private void StopButtonAction()
        {
            Model.EventManager.Invoke("StopSpin");
            UpdateStopButton();
        }

        private void UpdateStartButton()
        {
            bool canStart = Settings.Model.GetBool("CanStart");
            Debug.Log($"[UIButtonBinder] CanStart изменился: {canStart}");
            startButton.gameObject.SetActive(canStart);
        }

        private void UpdateStopButton()
        {
            bool canStop = Settings.Model.GetBool("CanStop");
            Debug.Log($"[UIButtonBinder] CanStop изменился: {canStop}");
            stopButton.gameObject.SetActive(canStop);
        }


    }
}
