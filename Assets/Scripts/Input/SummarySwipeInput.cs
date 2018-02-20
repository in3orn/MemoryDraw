using UnityEngine;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class SummarySwipeInput : GameStateDependentInput
    {
        [Header("Settings")]
        [SerializeField]
        private float MinSwipeLength = 10.0f;

        [Header("Dependencies")]
        [SerializeField]
        private ThemeController themeController;

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
                if (ax > MinSwipeLength)
                {
                    if (swipe.x > 0.0F)
                    {
                        themeController.NextTheme();
                        return;
                    }
                    if (swipe.x < 0.0F)
                    {
                        themeController.PrevTheme();
                        return;
                    }
                }
            }
        }

        protected abstract bool IsInputDown();

        protected abstract bool IsInputUp();
    }
}