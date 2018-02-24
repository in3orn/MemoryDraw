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


        public void Init(int id, ThemeData themeData, GroupData groupData, GroupProgressData progressData)
        {
            Init(id);

            for(int i = 0; i < mainImages.Length; i++)
            {
                mainImages[i].sprite = Resources.Load<Sprite>("Drawings/" + groupData.Images[i]);
            }

            background.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond);
            border.color = themeData.GetColor(ThemeData.ColorEnum.Main);
            
            locked = !progressData.Unlocked;
            locker.enabled = locked;

            foreach (var starBorder in starBorders)
            {
                starBorder.color = themeData.GetColor(ThemeData.ColorEnum.Main);
            }

            for (int i = 0; i < starFills.Length; i++)
            {
                var starFill = starFills[i];
                starFill.color = themeData.GetColor(i < CalculateGroupStars(groupData) ? ThemeData.ColorEnum.Main : ThemeData.ColorEnum.BkgSecond);
            }
        }

        private float CalculateGroupStars(GroupData groupData)
        {
            int stars = 0;

            foreach(var drawingData in groupData.Drawings)
            {
                stars += drawingData.Stars;
            }

            return stars / groupData.Drawings.Length;
        }
    }

}