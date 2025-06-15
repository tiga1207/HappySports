using UnityEngine;
using System;

public class BeatSoundManager : MonoBehaviour
{
    public float bpm = 105f;
    public float leadTime = 1.7f; // 미리 나와야 하는 시간
    public AudioSource musicSource;
    private float beat;
    private float timer;
    private bool isEnd = false;

    public event Action OnBeat; // 스폰 타이밍 이벤트
    public event Action OnMusicEnd; //음악종료 이벤트


    void Start()
    {
        beat = 60f / bpm;
        timer = 0f;
    }

    void Update()
    {
        if (!musicSource.isPlaying && musicSource.time > 0f && !isEnd)
        {
            isEnd = true;

            //음악 종료시 이벤트 발생
            OnMusicEnd?.Invoke();
        }

        if (!musicSource.isPlaying) return;

        timer += Time.deltaTime;
        if (timer >= beat)
        {
            timer -= beat;
            OnBeat?.Invoke(); // 스포너에서 큐브 생성
        }
    }

    public float GetRemainTime() =>
        musicSource.clip.length - musicSource.time;
}
