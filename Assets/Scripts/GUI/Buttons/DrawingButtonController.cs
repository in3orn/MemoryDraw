using Dev.Krk.MemoryDraw.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class DrawingButtonController : ButtonController
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image mainImage;

        [SerializeField]
        private Image border;

        [SerializeField]
        private Image[] starBorders;

        [SerializeField]
        private Image[] starFills;


        public void Init(int id, ThemeData themeData, DrawingData drawingData)
        {
            Init(id);

            mainImage.sprite = Resources.Load<Sprite>("Drawings/" + drawingData.Image);

            background.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond);
            border.color = themeData.GetColor(ThemeData.ColorEnum.Main);

            foreach (var starBorder in starBorders)
            {
                starBorder.color = themeData.GetColor(ThemeData.ColorEnum.Main);
            }

            for (int i = 0; i < starFills.Length; i++)
            {
                var starFill = starFills[i];
                starFill.color = themeData.GetColor(i < drawingData.Stars ? ThemeData.ColorEnum.Main : ThemeData.ColorEnum.BkgSecond);
            }
        }
    }

}