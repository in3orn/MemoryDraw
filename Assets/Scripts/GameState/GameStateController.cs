﻿using System.Collections;
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
        private GroupButtonsController groupButtonsController;

        [SerializeField]
        private DrawingButtonsController drawingButtonsController;


        private StateEnum state;

        private int currentGroup;


        private StateEnum State
        {
            set
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

            groupButtonsController.OnClicked += ProcessGroupButtonClicked;

            drawingButtonsController.OnClicked += ProcessDrawingButtonClicked;
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
                groupButtonsController.OnClicked -= ProcessGroupButtonClicked;
            }


            if (drawingButtonsController != null)
            {
                drawingButtonsController.OnClicked -= ProcessDrawingButtonClicked;
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
            StartCoroutine(ChangeState(StateEnum.Drawings, null, flowCompletedDelay));
        }

        private void ProcessLevelFailed()
        {
            StartCoroutine(ChangeState(StateEnum.Drawings, summary.Show, levelFailedDelay));
        }

        private void ProcessResourcesInitialized()
        {
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

        private void ProcessGroupButtonClicked(int id)
        {
            currentGroup = id;
            drawingButtonsController.Init(currentGroup);
            State = StateEnum.Drawings;
        }

        private void ProcessDrawingButtonClicked(int id)
        {
            drawingController.SetDrawing(currentGroup, id);
            progressController.StartDrawing(currentGroup, id);
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