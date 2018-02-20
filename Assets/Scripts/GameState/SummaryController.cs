using UnityEngine;
using Dev.Krk.MemoryDraw.Game.State;
using System.Collections;

namespace Dev.Krk.MemoryDraw.Summary
{
    public class SummaryController : MonoBehaviour
    {
        private enum StateEnum
        {
            Idle,
            Menu
        }

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private float transferScoreDelay;

        [SerializeField]
        private PopUpCanvas summary;

        private StateEnum state;

        void Start()
        {
            state = StateEnum.Idle;
        }

        public void Show()
        {
            summary.Show();
            StartCoroutine(TransferScore());
        }

        private IEnumerator TransferScore()
        {
            yield return new WaitForSeconds(transferScoreDelay);

            scoreController.TransferScore();
        }

        public void Hide()
        {
            summary.Hide();
        }

        public void OnPlayPressed()
        {
            state = StateEnum.Idle;
            Hide();
        }

        public void OnSettingsPressed()
        {
            Hide();
        }

        public void OnShopPressed()
        {
            Hide();
        }
    }
}