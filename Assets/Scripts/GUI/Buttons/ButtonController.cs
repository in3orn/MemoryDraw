using System;
using Dev.Krk.MemoryDraw.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class ButtonController : MonoBehaviour
    {
        public UnityAction<int> OnClicked;


        [SerializeField]
        private Image mainImage;

        [SerializeField]
        private Image borderImage;

        [SerializeField]
        private Image[] starBorders;

        [SerializeField]
        private Image[] starFills;


        private int id;


        public int Id { get { return id; } }


        void Start()
        {

        }


        public void Click()
        {
            OnClicked(Id);
        }

        public void Init(int id, ThemeData themeData, Sprite sprite) //TODO add drawing data
        {
            this.id = id;

            mainImage.sprite = sprite;

            borderImage.color = themeData.GetColor(ThemeData.ColorEnum.Main);

            foreach(var starBorder in starBorders)
                starBorder.color = themeData.GetColor(ThemeData.ColorEnum.Main);

            foreach (var starFill in starFills)
                starFill.color = themeData.GetColor(ThemeData.ColorEnum.BkgSecond); //TODO depends on how much stars player get
        }
    }

}