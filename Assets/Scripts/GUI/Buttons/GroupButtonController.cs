using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class GroupButtonController : ButtonController
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image[] mainImages;

        [SerializeField]
        private Image locker;

        [SerializeField]
        private Image border;

        [SerializeField]
        private Image[] starBorders;

        [SerializeField]
        private Image[] starFills;

        [SerializeField]
        private Image progressBarBkg;

        [SerializeField]
        private Image progressBarFill;

        [SerializeField]
        private Image progressBarBorder;


        public void Init(int id, ThemeData themeData, GroupData groupData, GroupProgressData progressData)
        {
            Init(id);

            for (int i = 0; i < mainImages.Length; i++)
            {
                mainImages[i].sprite = Resources.Load<Sprite>("Drawings/" + groupData.Images[i]);
            }

            background.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond);
            border.color = themeData.GetColor(ThemeData.ColorEnum.Second);

            locked = !progressData.Unlocked;
            locker.enabled = locked;

            progressBarBkg.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond);
            progressBarFill.color = themeData.GetColor(ThemeData.ColorEnum.Main);
            progressBarBorder.color = themeData.GetColor(ThemeData.ColorEnum.Second);

            progressBarFill.fillAmount = CalculateGroupFill(progressData);

            foreach (var starBorder in starBorders)
            {
                starBorder.color = themeData.GetColor(ThemeData.ColorEnum.Second);
            }

            for (int i = 0; i < starFills.Length; i++)
            {
                var starFill = starFills[i];
                starFill.color = themeData.GetColor(i < CalculateGroupStars(progressData) ? ThemeData.ColorEnum.Main : ThemeData.ColorEnum.BkgSecond);
            }
        }

        private float CalculateGroupStars(GroupProgressData groupData)
        {
            float stars = 0;

            foreach (var drawingData in groupData.Drawings)
            {
                stars += drawingData.Stars;
            }

            return stars / groupData.Drawings.Count;
        }

        private float CalculateGroupFill(GroupProgressData groupData)
        {
            float fill = 0;

            foreach (var drawingData in groupData.Drawings)
            {
                if (drawingData.Completed) fill++;
            }

            return fill / groupData.Drawings.Count;
        }
    }

}