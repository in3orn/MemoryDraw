using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class MenuSwipeInput : MenuInput
    {
        [Header("Settings")]
        [SerializeField]
        private float MinSwipeLength = 10.0f;
        

        private Vector2 start;

        private bool down;
        

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
                        OnPrevActionLaunched();
                        return;
                    }
                    if (swipe.x < 0.0F)
                    {
                        OnNextActionLaunched();
                        return;
                    }
                }
            }
        }

        protected abstract bool IsInputDown();

        protected abstract bool IsInputUp();
    }
}