using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Game
{
    public class GameController : MonoBehaviour
    {
        public UnityAction OnFlowCompleted;
        public UnityAction OnLevelFailed;


        [SerializeField]
        private LevelController levelController;
        
        [SerializeField]
        private ProgressController progressController;

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private LivesController livesController;


        void Start()
        {
        }

        void OnEnable()
        {
            levelController.OnLevelCompleted += ProcessLevelCompleted;
            levelController.OnLevelFailed += ProcessLevelFailed;

            levelController.OnFlowCompleted += ProcessFlowCompleted;
            levelController.OnPlayerFailed += ProcessPlayerDied;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelCompleted -= ProcessLevelCompleted;
                levelController.OnLevelFailed -= ProcessLevelFailed;

                levelController.OnFlowCompleted -= ProcessFlowCompleted;
                levelController.OnPlayerFailed -= ProcessPlayerDied;
            }
        }

        public void StartNewRun()
        {
            ResetLevel();
            StartLevel();
        }


        public void ProcessLevelFailed()
        {
            progressController.ResetFlow(scoreController.Level);

            if (OnLevelFailed != null) OnLevelFailed();
        }

        private void ProcessLevelCompleted()
        {
            scoreController.IncreaseScore();
            progressController.NextMap();

            if (progressController.IsFlowCompleted())
            {
                levelController.CompleteFlow();
            }
            else
            {
                StartCoroutine(StartLevelWithDelay());
            }
        }

        private void ProcessFlowCompleted()
        {
            progressController.NextFlow(scoreController.Level);
            if (OnFlowCompleted != null) OnFlowCompleted();
        }

        private IEnumerator StartLevelWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            StartLevel();
        }

        public void ProcessPlayerDied()
        {
            if (progressController.Flow > 0)
            {
                livesController.DecreaseLives();
                if (livesController.Lives <= 0)
                {
                    levelController.FailLevel();
                }
            }
        }

        private void ResetLevel()
        {
            livesController.ResetLives();
            levelController.Reset();
        }

        private void StartLevel()
        {
            levelController.Clear();
            levelController.Init(progressController.Flow, progressController.Map);
        }

        public void MoveLeft()
        {
            levelController.MoveLeft();
        }

        public void MoveRight()
        {
            levelController.MoveRight();
        }

        public void MoveUp()
        {
            levelController.MoveUp();
        }

        public void MoveDown()
        {
            levelController.MoveDown();
        }
    }
}