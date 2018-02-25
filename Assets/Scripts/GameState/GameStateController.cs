using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryDraw.Game;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.Summary;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.GUI.Buttons;
using Dev.Krk.MemoryDraw.Game.Drawing;

namespace Dev.Krk.MemoryDraw.State
{
    public class GameStateController : MonoBehaviour
    {
        public enum StateEnum
        {
            Idle = 0,
            Gameplay,
            Groups,
            Drawings,
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


        [Header("Dependencies")]
        [SerializeField]
        private ScoreController scoreController;

        [SerializeField]
        private DrawingController drawingController;

        [SerializeField]
        private ProgressController progressController;

        [SerializeField]
        private ButtonsController groupButtonsController;

        [SerializeField]
        private ButtonsController drawingButtonsController;


        private StateEnum state;

        private int currentGroup;

        public StateEnum State
        {
            get { return state; }
            private set
            {
                if (state != value)
                {
                    state = value;
                    OnStateChanged(state);
                }
            }
        }

        void OnEnable()
        {
            initializer.OnInitialized += ProcessResourcesInitialized;

            game.OnFlowCompleted += ProcessFlowCompleted;
            game.OnLevelFailed += ProcessLevelFailed;

            groupButtonsController.OnButtonClicked += ProcessGroupButtonClicked;

            drawingButtonsController.OnButtonClicked += ProcessDrawingButtonClicked;
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

            if (groupButtonsController != null)
            {
                groupButtonsController.OnButtonClicked -= ProcessGroupButtonClicked;
            }


            if (drawingButtonsController != null)
            {
                drawingButtonsController.OnButtonClicked -= ProcessDrawingButtonClicked;
            }
        }

        void Start()
        {
            initializer.Init();
        }

        public void PlayGame()
        {
            State = StateEnum.Gameplay;
            game.StartNewRun();
        }

        public void ShowGroups()
        {
            State = StateEnum.Groups;
        }

        public void ShowSettings()
        {
            State = StateEnum.Settings;
        }

        private void ProcessFlowCompleted()
        {
            drawingButtonsController.UpdateButton(progressController.DrawingIndex);
            groupButtonsController.UpdateButton(progressController.GroupIndex);

            StartCoroutine(ChangeState(StateEnum.Drawings, null, flowCompletedDelay));
        }

        private void ProcessLevelFailed()
        {
            StartCoroutine(ChangeState(StateEnum.Drawings, summary.Show, levelFailedDelay));
        }

        private void ProcessResourcesInitialized()
        {
            groupButtonsController.Init(0); //TODO last group from player prefs
            if (true)//scoreController.Level > 0)
            {
                State = StateEnum.Groups;
            }
            else
            {
                drawingController.SetDrawing(0, 0);
                progressController.StartDrawing(0, 0);
                PlayGame();
            }
        }

        private void ProcessGroupButtonClicked(ButtonController button)
        {
            if (!button.Locked)
            {
                currentGroup = button.Id;
                drawingButtonsController.Init(0, currentGroup);
                State = StateEnum.Drawings;
            }
            else
            {
                //TODO open in-game store
                Debug.Log("Store opened");
            }
        }

        private void ProcessDrawingButtonClicked(ButtonController button)
        {
            drawingController.SetDrawing(currentGroup, button.Id);
            progressController.StartDrawing(currentGroup, button.Id);
            PlayGame();
        }

        private IEnumerator ChangeState(StateEnum state, StateChangeAction action, float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);

            State = state;
            if (action != null) action();
        }
    }
}