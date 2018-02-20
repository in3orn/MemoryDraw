﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Data;

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

        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;


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
            progressController.StartDrawing(0, 0); //TODO group and drawing taken from GUI (drawing button)
            ResetLevel();
            StartLevel();
        }


        public void ProcessLevelFailed()
        {
            if (OnLevelFailed != null) OnLevelFailed();
        }

        private void ProcessLevelCompleted()
        {
            scoreController.IncreaseScore();
            progressController.NextMap();

            if (progressController.IsDrawingCompleted())
            {
                levelController.CompleteFlow();
            }
            else
            {
                StartCoroutine(StartWithDelay());
            }
        }

        private void ProcessFlowCompleted()
        {
            progressController.FinishDrawing();
            if (OnFlowCompleted != null) OnFlowCompleted();
        }

        private IEnumerator StartWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            StartLevel();
        }

        public void ProcessPlayerDied()
        {
            if (progressController.Drawing > 0)
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
            levelController.Init(GetMapData(progressController.Group, progressController.Drawing, progressController.Map));
        }

        private MapData GetMapData(int group, int drawing, int map)
        {
            GroupData groupData = groupsDataInitializer.Data.Groups[group];
            DrawingData drawingData = groupData.Drawings[drawing];
            return drawingData.Maps[map];
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