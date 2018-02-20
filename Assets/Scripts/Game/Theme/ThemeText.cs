using UnityEngine;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.Game.Theme
{
    [RequireComponent(typeof(Text))]
    public class ThemeText : ThemeObject
    {
        private Text text;

        void Awake()
        {
            text = GetComponent<Text>();
        }

        protected override void UpdateColor()
        {
            Color color = GetColor();
            if(text.color != color)
            {
                text.color = new Color(color.r, color.g, color.b);
            }
        }
    }
}
