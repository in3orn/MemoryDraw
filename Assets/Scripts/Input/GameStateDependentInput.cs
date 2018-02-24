using UnityEngine;
using Dev.Krk.MemoryDraw.State;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class GameStateDependentInput : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private GameStateController.StateEnum[] allowedStates;


        [Header("Dependencies")]
        [SerializeField]
        private GameStateController gameStateController;


        private bool running;


        void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            enabled = IsSupported();
        }

        protected abstract bool IsSupported();



        void OnEnable()
        {
            gameStateController.OnStateChanged += ProcessStateChanged;
        }

        void OnDisable()
        {
            if (gameStateController != null)
            {
                gameStateController.OnStateChanged -= ProcessStateChanged;
            }
        }

        private void ProcessStateChanged(GameStateController.StateEnum state)
        {
            running = IsStateAllowed(state);
        }

        private bool IsStateAllowed(GameStateController.StateEnum state)
        {
            foreach (var allowedState in allowedStates)
                if (state == allowedState) return true;

            return false;
        }


        void Update()
        {
            if (running)
            {
                UpdateInput();
            }
        }

        protected abstract void UpdateInput();
    }
}