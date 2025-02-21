using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using TMPro;
using UnityEngine;

namespace LootboxFSM
{
    public class LootboxController : MonoBehaviourExtBind
    {
        public TextMeshProUGUI winText;
        public ParticleSystem confetti;
        public static LootboxController instanse;

        [OnAwake]
        private void StartThis()
        {
            instanse = this;
            Log.Debug("LootboxController Start");
            Settings.Fsm = new FSM();
            Settings.Fsm.Add(new IdleState());
            Settings.Fsm.Add(new SpinningState());
            Settings.Fsm.Add(new StoppingState());

            Settings.Fsm.Start("Idle");

            Settings.Model.EventManager.AddAction("CheckWinCondition", CheckWinCondition);
            winText.gameObject.SetActive(false);
            confetti.Stop();
        }

        [OnUpdate]
        private void UpdateThis()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }

        [Bind("StartSpin")]
        public void StartSpin()
        {
            if (Settings.Model.GetBool("CanStart"))
            {
                Settings.Fsm.Change("Spinning");
            }
        }

        [Bind("StopSpin")]
        public void StopSpin()
        {
            if (Settings.Model.GetBool("CanStop"))
            {
                Settings.Fsm.Change("Stopping");
            }
        }

        private void CheckWinCondition()
        {
            string result1 = Settings.Model.Get<string>("SlotResult0", "");
            string result2 = Settings.Model.Get<string>("SlotResult1", "");
            string result3 = Settings.Model.Get<string>("SlotResult2", "");

            Debug.Log($"Проверка выигрыша: {result1}, {result2}, {result3}");

            if (!string.IsNullOrEmpty(result1) && result1 == result2 && result2 == result3)
            {
                Debug.Log("Выигрышная комбинация!");

                winText.gameObject.SetActive(true);
                winText.text = "Вы выиграли!";
                confetti.Play();
            }
            else
            {
                Debug.Log("Комбинация не выигрышная.");
            }
        }

    }
}
