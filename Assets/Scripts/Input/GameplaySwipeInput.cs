using UnityEngine;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class GameplaySwipeInput : GameplayInput
    {
        [Header("Settings")]
        [SerializeField]
        private float MinSwipeLength;

        [SerializeField]
        private float MinDirectionDifference;


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
                float ay = Mathf.Abs(swipe.y);
                if (ax > MinSwipeLength && ax - ay > MinDirectionDifference)
                {
                    if (swipe.x > 0.0F)
                    {
                        OnMoveRightActionLaunched();
                        return;
                    }
                    if (swipe.x < 0.0F)
                    {
                        OnMoveLeftActionLaunched();
                        return;
                    }
                }
                if (ay > MinSwipeLength && ay - ax > MinDirectionDifference)
                {
                    if (swipe.y > 0.0F)
                    {
                        OnMoveUpActionLaunched();
                        return;
                    }
                    if (swipe.y < 0.0F)
                    {
                        OnMoveDownActionLaunched();
                        return;
                    }
                }
            }
        }

        protected abstract bool IsInputDown();

        protected abstract bool IsInputUp();
    }
}