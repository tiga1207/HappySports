using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using TMPro;
using Util;
using System.IO;
using System;

public class GameManager : CustomSingleton<GameManager>
{
    [Header("Managers")]
    public NoteSpawner noteSpawner;

    [Header("XR References")]
    [SerializeField] private XROrigin xROrigin;
    [SerializeField] private GameObject startPointTarget;
    [SerializeField] private GameObject locomotionSys;

    [Header("UI & Controllers")]
    [SerializeField] private GameObject songSelectUI;
    [SerializeField] private XRBaseController leftController;
    [SerializeField] private XRBaseController rightController;
    [SerializeField] private GameObject[] ControllerInteractor; //좌우 컨트롤러 인터렉트 전부
    [SerializeField] private GameObject leftSaber;
    [SerializeField] private GameObject rightSaber;
    [SerializeField] private TextMeshProUGUI timeText;
    //이벤트
    public event Action OnGameStart;


    private void OnEnable()
    {
        SoundManager.Instance.OnMusicEnd += EndGame;
    }

    private void OnDisable()
    {
        SoundManager.Instance.OnMusicEnd -= EndGame;
    }

    private void Update()
    {
        UpdateRemainTimeUI();
    }

    public void StartGame(SongFolder selectedSong, string difficulty)
    {
        // 음악 로드 및 실행
        string audioPath = $"Songs/{selectedSong.folderName}/{Path.GetFileNameWithoutExtension(selectedSong.info.songFilename)}";
        AudioClip clip = Resources.Load<AudioClip>(audioPath);
        if (clip == null)
        {
            Debug.Log("오디오 파일을 불러올 수 없습니다.");
            return;
        }
        SoundManager.Instance.LoadAndPlay(clip, selectedSong.info.bpm);

        // 맵 로드
        var diff = selectedSong.info.difficultyBeatmaps.Find(d => d.difficulty == difficulty);
        string mapPath = $"Songs/{selectedSong.folderName}/{Path.GetFileNameWithoutExtension(diff.beatmapFilename)}";
        noteSpawner.LoadBeatMapFromPath(mapPath, selectedSong.info.bpm);

        //이벤트 발행
        OnGameStart?.Invoke();

        // introducUI.SetActive(false);

        #region XR 초기화

        locomotionSys.SetActive(false);
        leftController.model.gameObject.SetActive(false);
        rightController.model.gameObject.SetActive(false);

        foreach (var go in ControllerInteractor)
        {
            go.SetActive(false);
        }

        leftSaber.SetActive(true);
        rightSaber.SetActive(true);
        #endregion

        //카메라 역회전
        Quaternion inverse = Quaternion.Inverse(xROrigin.Camera.transform.rotation);
        xROrigin.transform.rotation = inverse;
        //카메라 위치 이동
        Vector3 targetPos = startPointTarget.transform.position;
        xROrigin.MoveCameraToWorldLocation(new Vector3(targetPos.x, xROrigin.CameraYOffset, targetPos.z));
    }

    private void EndGame()
    {
        locomotionSys.SetActive(true);
        leftController.model.gameObject.SetActive(true);
        rightController.model.gameObject.SetActive(true);
        foreach (var go in ControllerInteractor)
        {
            go.SetActive(true);
        }
        leftSaber.SetActive(false);
        rightSaber.SetActive(false);
        
        // 노래선택 스크롤뷰 활성화
        songSelectUI.SetActive(true);
    }

    private void UpdateRemainTimeUI()
    {
        float remaining = SoundManager.Instance.GetRemainingTime();
        int min = Mathf.FloorToInt(remaining / 60);
        int sec = Mathf.FloorToInt(remaining % 60);
        timeText.text = $"{min:00}:{sec:00}";
    }

    public void SendHapticFeedback(bool isLeft, float amplitude, float duration)
    {
        var controller = isLeft ? leftController : rightController;
        controller?.SendHapticImpulse(amplitude, duration);
    }

}
