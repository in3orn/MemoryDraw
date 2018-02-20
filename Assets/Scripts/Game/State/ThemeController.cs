using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Resources;

namespace Dev.Krk.MemoryDraw.Game.State
{
    public class ThemeController : ResourcesInitializer
    {
        public UnityAction OnThemeUpdated;
        
        private readonly string THEME= "Theme";

        [SerializeField]
        private ThemeData[] themes;

        private int theme;
        

        public int Theme { get { return theme; } }


        public ThemeData GetCurrentTheme()
        {
            return themes[theme];
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveData();
        }

        void OnApplicationQuit()
        {
            SaveData();
        }

        public override void Init()
        {
            LoadData();
            initialized = true;
            if (OnInitialized != null) OnInitialized();
        }

        public void NextTheme()
        {
            if(theme < themes.Length - 1)
            {
                theme++;

                if (OnThemeUpdated != null) OnThemeUpdated();
            }
        }

        public void PrevTheme()
        {
            if (theme > 0)
            {
                theme--;

                if (OnThemeUpdated != null) OnThemeUpdated();
            }
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(THEME, themes[theme].Id);
        }

        private void LoadData()
        {
            int themeId = PlayerPrefs.GetInt(THEME);
            theme = GetThemeIndex(themeId);
        }

        private int GetThemeIndex(int id)
        {
            for(int i = 0; i < themes.Length; i++)
            {
                ThemeData theme = themes[i];
                if (theme.Id == id)
                    return i;
            }
            return -1;
        }
    }
}