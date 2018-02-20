using UnityEngine;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.Game.Theme
{
    [RequireComponent(typeof(Camera))]
    public class ThemeCamera : ThemeObject
    {
        private Camera theCamera;

        void Awake()
        {
            theCamera = GetComponent<Camera>();
        }

        protected override void UpdateColor()
        {
            Color color = GetColor();
            if (theCamera.backgroundColor != color)
            {
                theCamera.backgroundColor = new Color(color.r, color.g, color.b);
            }
        }
    }
}
