﻿using UnityEngine;

namespace Dev.Krk.MemoryDraw.Game.State
{
    public class TutorialController : MonoBehaviour
    {
        private enum StateEnum
        {
            Hidden = 0,
            SwipeRight,
            SwipeUp,
            NextLevel,
            RememberPath
        }


        [Header("Settings")]
        [SerializeField]
        private float delay;

        [SerializeField]
        Animator[] animators;


        [Header("Dependencies")]
        [SerializeField]
        private LevelController levelController;

        [SerializeField]
        private ProgressController progressController;


        private float startTime;

        private bool running;

        private StateEnum state;

        private StateEnum State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;

                    foreach (var animator in animators)
                        animator.SetInteger("state", (int)state);
                }
            }
        }


        void Start()
        {
        }


        void OnEnable()
        {
            levelController.OnLevelStarted += ProcessLevelStarted;
            levelController.OnPlayerMoved += ProcessPlayerMoved;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnLevelStarted -= ProcessLevelStarted;
                levelController.OnPlayerMoved -= ProcessPlayerMoved;
            }
        }

        private void ProcessLevelStarted()
        {
            if (ShouldShowTutorial())
            {
                if (State == StateEnum.Hidden && progressController.MapIndex <= 0)
                {
                    State++;
                }
                else if(State == StateEnum.NextLevel)
                {
                    State++;
                }
            }
        }

        private void ProcessPlayerMoved(Vector2 direction)
        {
            if (ShouldShowTutorial())
            {
                if (State == StateEnum.SwipeRight && direction == Vector2.right)
                {
                    State++;
                }
                else if (State == StateEnum.SwipeUp && direction == Vector2.up)
                {
                    State++;
                }
                else if(State == StateEnum.RememberPath)
                {
                    State = StateEnum.Hidden;
                }
            }
        }

        private bool ShouldShowTutorial()
        {
            return progressController.GroupIndex <= 0 && progressController.DrawingIndex <= 0;
        }
    }
}
