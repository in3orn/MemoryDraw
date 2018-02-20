using UnityEngine;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.Game.Theme
{
    [RequireComponent(typeof(Image))]
    public class ThemeImage : ThemeObject
    {
        private Image image;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        protected override void UpdateColor()
        {
            Color color = GetColor();
            if (image.color != color)
            {
                image.color = new Color(color.r, color.g, color.b);
            }
        }
    }
}
