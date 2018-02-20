using System;
using Dev.Krk.MemoryDraw.Sounds;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.GUI
{
    public class SoundButtonController : MonoBehaviour
    {
        private const int MUSIC_ON = 0;
        private const int SOUND_ON = 1;
        private const int SOUND_OFF = 2;

        [SerializeField]
        private StateButton button;

        [SerializeField]
        private SoundSettingsController soundSettings;

        void Start()
        {

        }

        void OnEnable()
        {
            button.OnStateChanged += ProcessButtonStateChanged;
            
            soundSettings.OnMusicMuted += ProcessMusicMuted;
            soundSettings.OnSoundMuted += ProcessSoundMuted;

            soundSettings.OnInitialized += ProcessSettingsInitialized;
        }

        void OnDisable()
        {
            if (button != null)
            {
                button.OnStateChanged -= ProcessButtonStateChanged;
            }

            if (soundSettings != null)
            {
                soundSettings.OnMusicMuted -= ProcessMusicMuted;
                soundSettings.OnSoundMuted -= ProcessSoundMuted;

                soundSettings.OnInitialized -= ProcessSettingsInitialized;
            }
        }

        private void ProcessButtonStateChanged(int state)
        {
            soundSettings.OnMusicMuted -= ProcessMusicMuted;
            soundSettings.OnSoundMuted -= ProcessSoundMuted;

            switch (state)
            {
                case MUSIC_ON:
                    soundSettings.SetSoundMuted(false);
                    soundSettings.SetMusicMuted(false);
                    break;
                case SOUND_ON:
                    soundSettings.SetSoundMuted(false);
                    soundSettings.SetMusicMuted(true);
                    break;
                case SOUND_OFF:
                    soundSettings.SetSoundMuted(true);
                    soundSettings.SetMusicMuted(true);
                    break;
            }

            soundSettings.OnMusicMuted += ProcessMusicMuted;
            soundSettings.OnSoundMuted += ProcessSoundMuted;
        }

        private void ProcessMusicMuted(bool muted)
        {
            if (muted)
            {
                if(soundSettings.SoundMuted)
                    button.SetState(SOUND_OFF);
                else
                    button.SetState(SOUND_ON);
            }
            else
            {
                button.SetState(MUSIC_ON);
            }
        }

        private void ProcessSoundMuted(bool muted)
        {
            if (muted)
            {
                button.SetState(SOUND_OFF);
            }
            else
            {
                if (soundSettings.MusicMuted)
                    button.SetState(SOUND_ON);
                else
                    button.SetState(MUSIC_ON);
            }
        }

        private void ProcessSettingsInitialized()
        {
            if(soundSettings.SoundMuted)
            {
                button.SetState(SOUND_OFF);
            }
            else if(soundSettings.MusicMuted)
            {
                button.SetState(SOUND_ON);
            }
            else
            {
                button.SetState(MUSIC_ON);
            }
        }
    }
}