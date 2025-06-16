using UnityEngine;
using System;
using Util;


public class SoundManager : CustomSingleton<SoundManager>
{

    public AudioSource musicSource;
    public float BPM { get; private set; }
    private bool isEnded = false;
    public event Action OnMusicEnd;

    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.time > 0f && !isEnded)
        {
            isEnded = true;
            OnMusicEnd?.Invoke();
        }
    }

    public void LoadAndPlay(AudioClip _clip, float _bpm)
    {
        musicSource.clip = _clip;
        BPM = _bpm;
        musicSource.time = 0;
        isEnded = false;
        musicSource.Play();
    }

    public void Stop()
    {
        musicSource.Stop();
    }

    public float GetRemainingTime()
    {
        if (musicSource.clip == null) return 0;
        return musicSource.clip.length - musicSource.time;
    }
}
