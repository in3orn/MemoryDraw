using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Progress;
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
        private Image questionMark;

        [SerializeField]
        private Image[] stars;


        public void Init(int id, ThemeData themeData, DrawingData drawingData, DrawingProgressData progressData)
        {
            Init(id);

            background.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond);

            if (progressData.Completed)
            {
                mainImage.sprite = Resources.Load<Sprite>("Drawings/" + drawingData.Image);
            }
            else
            {
                questionMark.color = themeData.GetColor(ThemeData.ColorEnum.Second);
            }

            questionMark.enabled = !progressData.Completed;
            mainImage.enabled = progressData.Completed;

            for (int i = 0; i < stars.Length; i++)
            {
                var star = stars[i];
                star.color = themeData.GetColor(i < progressData.Stars ? ThemeData.ColorEnum.Main : ThemeData.ColorEnum.Second);
            }
        }
    }

}