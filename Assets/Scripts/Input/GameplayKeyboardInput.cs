using UnityEngine;
using Dev.Krk.MemoryDraw.Game;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public class GameplayKeyboardInput : GameplayInput
    {
        protected override bool IsSupported()
        {
            return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
        }


        protected override void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnMoveUpActionLaunched();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnMoveDownActionLaunched();
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnMoveLeftActionLaunched();
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnMoveRightActionLaunched();
                return;
            }
        }
    }
}