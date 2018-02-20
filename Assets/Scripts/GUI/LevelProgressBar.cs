using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Game.GUI
{
    public class LevelProgressBar : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Image progressBar;
        
        [SerializeField]
        private Animator animator;

        void Start()
        {
        }

        void OnEnable()
        {
            scoreController.OnScoreTransferred += ProcessScoreTransferred;
            scoreController.OnInitialized += UpdateProgress;
        }

        void OnDisable()
        {
            if (scoreController != null)
            {
                scoreController.OnScoreTransferred -= ProcessScoreTransferred;
                scoreController.OnInitialized -= UpdateProgress;
            }
        }

        private void ProcessScoreTransferred()
        {
            UpdateProgress();
            AnimateScore("TransferScore");
        }

        private void UpdateProgress()
        {
            progressBar.fillAmount = Mathf.Clamp(scoreController.LevelScore / (float) scoreController.MaxLevelScore, 0f, 1f);
        }

        private void AnimateScore(string trigger)
        {
            animator.SetTrigger(trigger);
        }
    }
}
