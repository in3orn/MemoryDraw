using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        public enum SoundId
        {
            Slide1 = 0,
            Slide2,
            Slide3,
            Slide4,
            Button1,
            Button2,
            Button3,
            Failure,
            LevelComplete,
            LevelFailed,
            ScoreTransfer1,
            ScoreTransfer2,
            ScoreTransfer3
        }

        private AudioSource audioSource;

        [SerializeField]
        private SoundData[] sounds;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {

        }

        public void PlaySound(SoundId soundId)
        {
            SoundData data = sounds[(int)soundId];
            audioSource.PlayOneShot(data.AudioClip, data.Volume);
        }
    }
}