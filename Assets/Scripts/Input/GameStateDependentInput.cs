using UnityEngine;
using Dev.Krk.MemoryDraw.State;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class GameStateDependentInput : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private GameStateController.StateEnum activeState;

        [Header("Dependencies")]
        [SerializeField]
        private GameStateController gameStateController;

        private bool running;

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
            running = state == activeState;
        }

        void Start()
        {

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