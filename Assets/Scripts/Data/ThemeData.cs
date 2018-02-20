using Dev.Krk.MemoryDraw.Sounds;
using System;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class ThemeData
    {
        public enum ColorEnum
        {
            Main = 0,
            Second,
            BkgMain,
            BkgSecond
        }

        public int Id;

        [SerializeField]
        private Color[] colors;

        [SerializeField]
        private SoundData music;

        public bool Locked;

        public int UnlockLevel;

        //TODO other unlocks

        public SoundData Music { get { return music; } }

        public Color GetColor(ColorEnum color)
        {
            return colors[(int)color];
        }
    }
}