using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Game.Animations
{
    public class PerfectLevelController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Text text;

        [SerializeField]
        private string[] completedTexts;


        [Header("Dependencies")]
        [SerializeField]
        private LevelController levelController;

        [SerializeField]
        private LivesController livesController;

        [SerializeField]
        private ScoreController scoreController;


        private bool perfect;

        private int index;


        void OnEnable()
        {
            levelController.OnLevelStarted += ProcessLevelStarted;
            levelController.OnLevelCompleted += ProcessLevelCompleted;

            livesController.OnLivesUpdated += ProcessLivesUpdated;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelStarted -= ProcessLevelStarted;
                levelController.OnLevelCompleted -= ProcessLevelCompleted;
            }

            if (livesController != null)
            {
                livesController.OnLivesUpdated -= ProcessLivesUpdated;
            }
        }

        private void ProcessLevelStarted()
        {
            perfect = true;
        }

        private void ProcessLevelCompleted()
        {
            if (perfect && (scoreController.Level > 0 || scoreController.CurrentScore > 2))
            {
                index += Random.Range(1, completedTexts.Length-1);
                index %= completedTexts.Length;
                text.text = completedTexts[index];
                animator.SetTrigger("OnShow");
            }
        }

        private void ProcessLivesUpdated()
        {
            perfect = livesController.Lives == livesController.StartLives;
        }
    }
}
