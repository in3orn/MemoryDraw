using UnityEngine;


namespace Dev.Krk.MemoryDraw.Inputs
{
    public class MenuKeyboardInput : MenuInput
    {
        protected override bool IsSupported()
        {
            return Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
        }

        protected override void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnPrevActionLaunched();
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnNextActionLaunched();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnMainActionLaunched();
                return;
            }
        }
    }
}