﻿using Dev.Krk.MemoryDraw.Common;
using Dev.Krk.MemoryDraw.Inputs;
using Dev.Krk.MemoryDraw.State;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class ButtonsController : MonoBehaviour
    {
        public UnityAction<ButtonController> OnButtonClicked;


        [Header("Settings")]
        [SerializeField]
        private float spacing;

        [SerializeField]
        private GameStateController.StateEnum[] allowedStates;


        [Header("Dependencies")]
        [SerializeField]
        private ButtonFactory buttonFactory;

        [SerializeField]
        private MenuInput menuInput;

        [SerializeField]
        private GameStateController gameStateController;


        private ButtonController[] buttons;

        private int currentId;


        void OnEnable()
        {
            Subscribe();
        }

        protected virtual void Subscribe()
        {
            SubscribeToButtons();
            SubscribeToInput();
        }

        protected void SubscribeToButtons()
        {
            if (buttons != null)
            {
                foreach (var button in buttons)
                {
                    button.OnClicked += ProcessButtonClicked;
                }
            }
        }

        private void SubscribeToInput()
        {
            menuInput.OnPrevActionLaunched += ProcessPrevActionLaunched;
            menuInput.OnNextActionLaunched += ProcessNextActionLaunched;
            menuInput.OnMainActionLaunched += ProcessMainActionLaunched;
        }

        void OnDisable()
        {
            Unsubscribe();
        }

        protected virtual void Unsubscribe()
        {
            UnsubscribeFromButtons();
            UnsubscribeFromInput();
        }

        private void UnsubscribeFromButtons()
        {
            if (buttons != null)
            {
                foreach (var button in buttons)
                {
                    if (button != null)
                    {
                        button.OnClicked -= ProcessButtonClicked;
                    }
                }
            }
        }

        private void UnsubscribeFromInput()
        {
            if (menuInput != null)
            {
                menuInput.OnPrevActionLaunched -= ProcessPrevActionLaunched;
                menuInput.OnNextActionLaunched -= ProcessNextActionLaunched;
                menuInput.OnMainActionLaunched -= ProcessMainActionLaunched;
            }
        }


        public void Init(int currentId, int parentId = 0)
        {
            Clear();

            this.currentId = currentId;

            buttons = buttonFactory.CreateButtons(parentId);

            foreach (var button in buttons) InitPosition(button);

            SubscribeToButtons();
        }

        public void Clear()
        {
            if (buttons != null)
            {
                UnsubscribeFromButtons();
                foreach (var button in buttons)
                {
                    Destroy(button.gameObject);
                }
                buttons = null;
            }
        }

        private void InitPosition(ButtonController buttonController)
        {
            int diff = buttonController.Id - currentId;
            float scale = Mathf.Pow(2, Mathf.Abs(diff));

            RectTransform rectTransform = buttonController.GetComponent<RectTransform>();
            rectTransform.SetParent(GetComponent<RectTransform>());
            rectTransform.anchoredPosition = Vector2.right * spacing * diff * scale;

            rectTransform.localScale = Vector3.one / scale;
        }


        private void ProcessButtonClicked(ButtonController button)
        {
            if (CanPerformAction())
            {
                if (button.Id < currentId)
                {
                    currentId--;
                    UpdatePositions();
                }
                else if (button.Id > currentId)
                {
                    currentId++;
                    UpdatePositions();
                }
                else
                {
                    OnButtonClicked(button);
                }
            }
        }

        private void ProcessPrevActionLaunched()
        {
            if (CanPerformAction() && currentId > 0)
            {
                currentId--;
                UpdatePositions();
            }
        }

        private void ProcessNextActionLaunched()
        {
            if (CanPerformAction() && currentId < buttons.Length - 1)
            {
                currentId++;
                UpdatePositions();
            }
        }

        private void ProcessMainActionLaunched()
        {
            if (CanPerformAction())
            {
                OnButtonClicked(buttons[currentId]);
            }
        }


        private bool CanPerformAction()
        {
            foreach (var allowedState in allowedStates)
            {
                if (gameStateController.State == allowedState)
                    return true;
            }

            return false;
        }


        private void UpdatePositions()
        {
            foreach (var button in buttons) UpdatePosition(button);
        }

        private void UpdatePosition(ButtonController buttonController)
        {
            int diff = buttonController.Id - currentId;
            float scale = Mathf.Pow(2, Mathf.Abs(diff));

            RectMover rectMover = buttonController.GetComponent<RectMover>();
            rectMover.Target = Vector2.right * spacing * diff * scale;

            RectScaler rectScaler = buttonController.GetComponent<RectScaler>();
            rectScaler.TargetScale = Vector3.one / scale;
        }
    }
}