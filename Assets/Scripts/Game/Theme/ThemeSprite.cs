using UnityEngine;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.Game.Theme
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ThemeSprite : ThemeObject
    {
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void UpdateColor()
        {
            Color color = GetColor();
            if (spriteRenderer.color != color)
            {
                spriteRenderer.color = new Color(color.r, color.g, color.b);
            }
        }
    }
}
