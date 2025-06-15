using UnityEngine;
using TMPro;
using System;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

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

    //게임 컨트롤러 모양
    [SerializeField] private XRBaseController leftController;
    [SerializeField] private XRBaseController rightController;

    //비트세이버
    [SerializeField] private GameObject leftSaber;
    [SerializeField] private GameObject rightSaber;

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

        //게임 시작 시 컨트롤러 모양 없애기
        leftController.model.gameObject.SetActive(false);
        rightController.model.gameObject.SetActive(false);
        //세이버 활성화
        leftSaber.SetActive(true);
        rightSaber.SetActive(true);
    }

    public void GameEnd()
    {
        //Locomotion 활성화
        locomotionSys.SetActive(true);
        //게임 설명 UI 활성화
        introducUI.SetActive(true);

        //게임 시작 시 컨트롤러 모양 없애기
        leftController.model.gameObject.SetActive(true); 
        rightController.model.gameObject.SetActive(true);
        //세이버 비활성화
        leftSaber.SetActive(false);
        rightSaber.SetActive(false);
    }

}