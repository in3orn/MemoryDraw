using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Game.GUI
{
    public class LevelScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private Text scoreLabel;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
        }
        
        void OnEnable()
        {
            scoreController.OnScoreTransferred += ProcessScoreTransferred;
            scoreController.OnInitialized += UpdateText;
        }

        void OnDisable()
        {
            if(scoreController != null)
            {
                scoreController.OnScoreTransferred -= ProcessScoreTransferred;
                scoreController.OnInitialized -= UpdateText;
            }
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
            scoreLabel.text = scoreController.LevelScore.ToString("#,0", format) + "/" + scoreController.MaxLevelScore.ToString("#,0", format);
        }

        private void AnimateScore(string trigger)
        {
            animator.SetTrigger(trigger);
        }
    }
}
