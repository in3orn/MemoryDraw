using UnityEngine;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public class MenuMobileSwipeInput : MenuSwipeInput
    {
        protected override void Init()
        {
            base.Init();

            Input.multiTouchEnabled = false;
        }

        protected override bool IsSupported()
        {
            return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }

        protected override bool IsInputDown()
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }

        protected override bool IsInputUp()
        {
            return Input.touchCount <= 0 || Input.GetTouch(0).phase == TouchPhase.Ended;
        }
    }
}