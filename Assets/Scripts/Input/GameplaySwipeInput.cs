using UnityEngine;
using Dev.Krk.MemoryDraw.Game;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class GameplaySwipeInput : GameStateDependentInput
    {
        [Header("Settings")]
        [SerializeField]
        private float MinSwipeLength = 10.0f;

        [SerializeField]
        private float MinSwipeDiff = 5.0f;

        [Header("Dependencies")]
        [SerializeField]
        private GameController gameController;

        private Vector2 start;

        private bool down;

        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            enabled = IsSupported();
        }

        protected abstract bool IsSupported();

        protected override void UpdateInput()
        {
            if (IsInputDown())
            {
                start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                down = true;
                return;
            }

            if (down && IsInputUp())
            {
                down = false;

                Vector2 end = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 swipe = end - start;

                float ax = Mathf.Abs(swipe.x);
                float ay = Mathf.Abs(swipe.y);
                if (ax > MinSwipeLength && ax - ay > MinSwipeDiff)
                {
                    if (swipe.x > 0.0F)
                    {
                        gameController.MoveRight();
                        return;
                    }
                    if (swipe.x < 0.0F)
                    {
                        gameController.MoveLeft();
                        return;
                    }
                }
                if (ay > MinSwipeLength && ay - ax > MinSwipeDiff)
                {
                    if (swipe.y > 0.0F)
                    {
                        gameController.MoveUp();
                        return;
                    }
                    if (swipe.y < 0.0F)
                    {
                        gameController.MoveDown();
                        return;
                    }
                }
            }
        }

        protected abstract bool IsInputDown();

        protected abstract bool IsInputUp();
    }
}