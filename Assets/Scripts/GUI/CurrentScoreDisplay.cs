using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryDraw.Game.State;
using System;

namespace Dev.Krk.MemoryDraw.Game.GUI
{
    public class CurrentScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Text scoreLabel;

        [SerializeField]
        private Animator[] animators;

        void Start()
        {
        }
        
        void OnEnable()
        {
            scoreController.OnScoreUpdated += ProcessScoreUpdated;
            scoreController.OnScoreTransferred += ProcessScoreTransferred;
        }

        void OnDisable()
        {
            if(scoreController != null)
            {
                scoreController.OnScoreUpdated -= ProcessScoreUpdated;
            }
        }

        private void ProcessScoreUpdated()
        {
            UpdateText();
            AnimateScore("UpdateScore");
        }

        private void ProcessScoreTransferred()
        {
            UpdateText();
            AnimateScore("TransferScore");
        }

        private void UpdateText()
        {
            NumberFormatInfo format = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
            format.NumberGroupSeparator = " ";
            scoreLabel.text = scoreController.CurrentScore.ToString("#,0", format);
        }

        private void AnimateScore(string trigger)
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger(trigger);
            }
        }
    }
}
