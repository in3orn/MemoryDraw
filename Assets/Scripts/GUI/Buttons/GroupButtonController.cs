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
        private Image[] stars;


        public void Init(int id, ThemeData themeData, GroupData groupData, GroupProgressData progressData)
        {
            Init(id);

            for (int i = 0; i < mainImages.Length; i++)
            {
                mainImages[i].sprite = Resources.Load<Sprite>("Drawings/" + groupData.Images[i]);
            }

            background.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond);

            locked = !progressData.Unlocked;
            locker.enabled = locked;
            
            for (int i = 0; i < stars.Length; i++)
            {
                var star = stars[i];
                star.color = themeData.GetColor(i+1 <= CalculateGroupStars(progressData) ? ThemeData.ColorEnum.Main : ThemeData.ColorEnum.Second);
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