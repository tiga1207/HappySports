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
    [SerializeField] private XROrigin xROrigin;
    [SerializeField] private GameObject startPointTarget;

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

        //카메라 회전
        Transform cam = xROrigin.Camera.transform;
        // 카메라의 현재 회전값 (월드 기준)
        Quaternion camRot = cam.rotation;
        // 그 회전의 역방향으로 XR Origin 보정하기
        Quaternion inverse = Quaternion.Inverse(camRot);
        xROrigin.transform.rotation = inverse;

        //XR 위치 이동
        Transform targetPos = startPointTarget.transform;
        xROrigin.MoveCameraToWorldLocation(new Vector3(targetPos.position.x, xROrigin.CameraYOffset, targetPos.position.z));

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
    public void sendHaptic(bool isLeft, float _amplitude, float _duration)
    {
        if (isLeft && leftController != null)
        {
            leftController.SendHapticImpulse(_amplitude, _duration);
        }
        else if (!isLeft && rightController != null)
        {
            rightController.SendHapticImpulse(_amplitude, _duration);
        }
    }

}