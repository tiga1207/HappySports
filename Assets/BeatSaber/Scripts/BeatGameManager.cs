using UnityEngine;
using TMPro;
using System;
using Unity.XR.CoreUtils;

public class BeatGameManager : MonoBehaviour
{
    public BeatSoundManager soundManager;
    public TextMeshProUGUI timeText;

    //카메라
    // [SerializeField] private Camera cam;
    // [SerializeField] private XROrigin xROrigin;

    //XR Locomotion 오브젝트
    public GameObject locomotionSys;

    //게임 설명 게임 오브젝트
    public GameObject introducUI;

    //게임 시작 이벤트
    public static event Action OnGameStart;
    void OnEnable()
    {
        soundManager.OnMusicEnd += GameEnd;
    }
    void OnDisable()
    {
        soundManager.OnMusicEnd -= GameEnd;
    }
    void Update()
    {
        float remaining = soundManager.GetRemainTime();
        int min = Mathf.FloorToInt(remaining / 60);
        int sec = Mathf.FloorToInt(remaining % 60);
        timeText.text = $"{min:00}:{sec:00}";
    }


    //게임 설명 UI 시작 버튼에 달아놨음.
    public void GameStartBtn()
    {
        soundManager.musicSource.Play();
        locomotionSys.SetActive(false);
        OnGameStart?.Invoke();

        //TODO: 게임 시작 시 카메라 위치 이동 -> 현재 XR 시뮬의 오류인지, 아니면 코드의 오류인지 판단이 어렵기에, 추후 테스트
        // cam.transform.position = new Vector3(0, 0, 0);;
        // xROrigin.MoveCameraToWorldLocation(new Vector3(0, 0, 0));
        // xROrigin.transform.position = new Vector3(0, 0, 0);
    }

    public void GameEnd()
    {
        //Locomotion 활성화
        locomotionSys.SetActive(true);
        //게임 설명 UI 활성화
        introducUI.SetActive(true);
    }

}