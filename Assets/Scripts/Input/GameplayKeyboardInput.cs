using UnityEngine;
using Dev.Krk.MemoryDraw.Game;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public class GameplayKeyboardInput : GameStateDependentInput
    {
        [Header("Dependencies")]
        [SerializeField]
        private GameController gameController;

        void Awake()
        {
            Init();
        }

        protected void Init()
        {
            enabled = IsSupported();
        }

        protected bool IsSupported()
        {
            return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
        }

        protected override void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gameController.MoveUp();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gameController.MoveDown();
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gameController.MoveLeft();
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gameController.MoveRight();
                return;
            }
        }
    }
}