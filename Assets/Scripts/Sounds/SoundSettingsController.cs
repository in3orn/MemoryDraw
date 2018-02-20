using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.Sounds
{
    public class SoundSettingsController : MonoBehaviour
    {
        public UnityAction<bool> OnSoundMuted;
        public UnityAction<bool> OnMusicMuted;

        public UnityAction OnInitialized;

        private readonly string MUSIC_MUTED = "MusicMuted";
        private readonly string SOUND_MUTED = "SoundMuted";

        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource soundSource;

        public bool SoundMuted {  get { return soundSource.mute; } }

        public bool MusicMuted { get { return musicSource.mute; } }

        void Start()
        {
            LoadData();

            if (OnInitialized != null)
                OnInitialized();
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

        private void SaveData()
        {
            PlayerPrefs.SetInt(MUSIC_MUTED, musicSource.mute ? 1 : 0);
            PlayerPrefs.SetInt(SOUND_MUTED, soundSource.mute ? 1 : 0);
        }

        private void LoadData()
        {
            musicSource.mute = PlayerPrefs.GetInt(MUSIC_MUTED) != 0;
            soundSource.mute = PlayerPrefs.GetInt(SOUND_MUTED) != 0;
        }

        public void SetSoundMuted(bool muted)
        {
            if (soundSource.mute != muted)
            {
                soundSource.mute = muted;
                if (OnSoundMuted != null)
                    OnSoundMuted(muted);
            }
        }

        public void SetMusicMuted(bool muted)
        {
            if (musicSource.mute != muted)
            {
                musicSource.mute = muted;
                if (OnMusicMuted != null)
                    OnMusicMuted(muted);
            }
        }
    }
}