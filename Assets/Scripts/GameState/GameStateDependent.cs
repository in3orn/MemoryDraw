using UnityEngine;

namespace Dev.Krk.MemoryDraw.State
{
    public class GameStateDependent : MonoBehaviour
    {
        [SerializeField]
        private GameStateController gameState;


        void OnEnable()
        {
            gameState.OnStateChanged += ProcessGameStateChanged;
        }

        void OnDisable()
        {
            if (gameState != null)
            {
                gameState.OnStateChanged -= ProcessGameStateChanged;
            }
        }

        private void ProcessGameStateChanged(GameStateController.StateEnum state)
        {
            foreach (var animator in GetComponentsInChildren<Animator>())
                animator.SetInteger("state", (int) state);
        }
    }
}