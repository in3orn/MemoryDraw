using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryDraw.Game;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.Summary;
using Dev.Krk.MemoryDraw.Resources;

namespace Dev.Krk.MemoryDraw.State
{
    public class GameStateController : MonoBehaviour
    {
        public enum StateEnum
        {
            Gameplay = 0,
            Summary,
            Settings
        }


        public UnityAction<StateEnum> OnStateChanged;


        private delegate void StateChangeAction();

        
        [Header("Settings")]
        [SerializeField]
        private float menuDelay;

        [SerializeField]
        private float flowCompletedDelay;

        [SerializeField]
        private float levelFailedDelay;


        [Header("Sections")]
        [SerializeField]
        private GameController game;

        [SerializeField]
        private SummaryController summary;

        [SerializeField]
        private SettingsController settings;

        [SerializeField]
        private ShopController shop;


        [Header("Resources")]
        [SerializeField]
        private ResourcesInitializer initializer;

        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private ProgressController progressController;


        void OnEnable()
        {
            initializer.OnInitialized += ProcessResourcesInitialized;

            game.OnFlowCompleted += ProcessFlowCompleted;
            game.OnLevelFailed += ProcessLevelFailed;
        }

        void OnDisable()
        {
            if (initializer != null)
            {
                initializer.OnInitialized -= ProcessResourcesInitialized;
            }

            if (game != null)
            {
                game.OnFlowCompleted -= ProcessFlowCompleted;
                game.OnLevelFailed -= ProcessLevelFailed;
            }
        }

        void Start()
        {
            initializer.Init();
        }

        public void PlayGame()
        {
            StartCoroutine(ChangeState(StateEnum.Gameplay, game.StartNewRun, menuDelay));
        }

        public void ShowSummary()
        {
            StartCoroutine(ChangeState(StateEnum.Summary, summary.Show, menuDelay));
        }

        public void ShowSettings()
        {
            StartCoroutine(ChangeState(StateEnum.Settings, settings.Show, menuDelay));
        }

        private void ProcessFlowCompleted()
        {
            StartCoroutine(ChangeState(StateEnum.Summary, summary.Show, flowCompletedDelay));
        }

        private void ProcessLevelFailed()
        {
            StartCoroutine(ChangeState(StateEnum.Summary, summary.Show, levelFailedDelay));
        }

        private void ProcessResourcesInitialized()
        {
            if (scoreController.Level > 0)
            {
                StartCoroutine(ChangeState(StateEnum.Summary, summary.Show, 0f));
            }
            else
            {
                PlayGame();
            }
        }

        private IEnumerator ChangeState(StateEnum state, StateChangeAction action, float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);

            action();

            if (OnStateChanged != null) OnStateChanged(state);
        }
    }
}