using UnityEngine;
using Dev.Krk.MemoryDraw.Game.State;
using System;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Sounds
{
    public class SoundThemeController : MonoBehaviour
    {
        [SerializeField]
        private ThemeController themeController;

        [SerializeField]
        private AudioSource musicSource;


        void Start()
        {
        }


        void OnEnable()
        {
            themeController.OnInitialized += ProcessThemeUpdated;
            themeController.OnThemeUpdated += ProcessThemeUpdated;
        }

        void OnDisable()
        {
            if (themeController != null)
            {
                themeController.OnInitialized -= ProcessThemeUpdated;
                themeController.OnThemeUpdated -= ProcessThemeUpdated;
            }
        }

        private void ProcessThemeUpdated()
        {
            ThemeData data = themeController.GetCurrentTheme();
            
            if(musicSource.clip != data.Music.AudioClip)
            {
                musicSource.volume = data.Music.Volume;
                musicSource.clip = data.Music.AudioClip;
                musicSource.Play();
            }
        }
    }
}