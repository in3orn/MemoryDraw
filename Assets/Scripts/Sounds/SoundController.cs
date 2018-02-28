using UnityEngine;
using Dev.Krk.MemoryDraw.Game;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Sounds
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        private SoundPlayer soundPlayer;

        [SerializeField]
        private LevelController levelController;

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private int numOfButtons = 3;
        private int buttonIndex;

        [SerializeField]
        private int numOfSlides = 4;
        private int slideIndex;

        [SerializeField]
        private int numOfScoreUpdates = 3;
        private int scoreUpdateIndex;

        private Vector2 prevVector;

        private float prevMoveTime;

        [SerializeField]
        private float firstMoveDelay;

        void Start()
        {

        }

        void OnEnable()
        {
            levelController.OnPlayerMoved += ProcessPlayerMoved;
            levelController.OnPlayerFailed += ProcessPlayerFailed;

            levelController.OnPartCompleted += ProcessLevelCompleted;
            levelController.OnLevelCompleted += ProcessLevelCompleted;
            levelController.OnLevelFailed += ProcessLevelFailed;

            scoreController.OnScoreTransferred += ProcessScoreTransferred;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnPlayerMoved -= ProcessPlayerMoved;
                levelController.OnPlayerFailed -= ProcessPlayerFailed;

                levelController.OnPartCompleted -= ProcessLevelCompleted;
                levelController.OnLevelCompleted -= ProcessLevelCompleted;
                levelController.OnLevelFailed -= ProcessLevelFailed;
            }

            if(scoreController != null)
            {
                scoreController.OnScoreTransferred -= ProcessScoreTransferred;
            }
        }

        public void PlayButtonPressed()
        {
            buttonIndex += Random.Range(0, numOfButtons - 1);
            buttonIndex %= numOfButtons;

            soundPlayer.PlaySound(SoundPlayer.SoundId.Button1 + buttonIndex);
        }

        public void PlayLockedButtonPressed()
        {
            soundPlayer.PlaySound(SoundPlayer.SoundId.Failure);
        }

        private void ProcessPlayerMoved(Vector2 vector)
        {
            slideIndex += Random.Range(0, numOfSlides - 1);
            slideIndex %= numOfSlides;

            if(IsDirectionChanged(vector) || IsFirstMove())
            {
                soundPlayer.PlaySound(SoundPlayer.SoundId.Slide1 + slideIndex);
            }

            prevVector = vector;
        }

        private bool IsDirectionChanged(Vector2 vector)
        {
            return (vector - prevVector).magnitude > 0.5f;
        }

        private bool IsFirstMove()
        {
            float moveTime = Time.time;
            bool result = (moveTime - prevMoveTime) > firstMoveDelay;
            prevMoveTime = moveTime;
            return result;
        }

        private void ProcessPlayerFailed()
        {
            soundPlayer.PlaySound(SoundPlayer.SoundId.Failure);
        }

        private void ProcessLevelCompleted()
        {
            prevVector = Vector2.zero;
            soundPlayer.PlaySound(SoundPlayer.SoundId.LevelComplete);
        }

        private void ProcessLevelFailed()
        {
            soundPlayer.PlaySound(SoundPlayer.SoundId.LevelFailed);
        }

        private void ProcessScoreTransferred()
        {
            scoreUpdateIndex += Random.Range(0, numOfScoreUpdates - 1);
            scoreUpdateIndex %= numOfScoreUpdates;

            soundPlayer.PlaySound(SoundPlayer.SoundId.ScoreTransfer1 + scoreUpdateIndex);
        }
    }
}