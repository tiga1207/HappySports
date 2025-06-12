using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BowlingBall : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable xrGrab;
    public Collider SafeTriggerZone;
    public Transform ballInitPos;

    //각 턴마다 초기화 대상
    [SerializeField] private bool isThrow;
    [SerializeField] private bool timerActive;
    private float timeAfterThrown = 1f;
    //이벤트
    public static UnityEvent OnPlayerThrow = new();
    public static UnityEvent OnBallStopped = new();

    void OnEnable()
    {
        BowlingGameManager.OnTurnReset.AddListener(InitBowlingBall);
    }
    void OnDisable()
    {
        BowlingGameManager.OnTurnReset.RemoveListener(InitBowlingBall);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        xrGrab = GetComponent<XRGrabInteractable>();
    }
    private void Update()
    {
        if (!isThrow || !timerActive)
            return;

        //타이머 시작
        timeAfterThrown -= Time.deltaTime;


        if (timeAfterThrown > 0f && SafeTriggerZone.bounds.Contains(transform.position))
        {
            //1초 이내로 공이 다시 공을 놓는 존에 돌아왔을 경우
            isThrow = false;
            timeAfterThrown = 1f;
            timerActive = false;
            xrGrab.enabled = true;
        }
        //진짜로 던지는 로직
        else if (timeAfterThrown <= 0f)
        {
            timerActive = false;
            OnPlayerThrow?.Invoke();
            StartCoroutine(IE_CheckSpeed());
        }
    }
    private IEnumerator IE_CheckSpeed()
    {
        //계속 체크를 해야하기 때문에 코루틴으로 관리
        while (rb.velocity.magnitude > 0f)
        {
            yield return null;
        }
        OnBallStopped?.Invoke();
    }


    //XR Origin의 Event에 걸기.
    public void IsThrow()
    {
        isThrow = true;
        timeAfterThrown = 1f;
        timerActive = true;

        //즉시 꺼버리면 힘이 적용이 안되기 때문에 한 프레임 늦게 꺼버리기.
        StartCoroutine(IE_NextFrame());
    }
    private IEnumerator IE_NextFrame()
    {
        //다시 그랩할 수 있도록 하는 것을 방지

        yield return null;
        xrGrab.enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            if (!isThrow)
            {
                //던지지 않았는데 공이 밖으로 나가는 경우 제자리에 두기
                InitBowlingBall();
            }
        }
    }

    private void InitBowlingBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        transform.position = ballInitPos.position;

        isThrow = false;

        xrGrab.enabled = true;
    }

}
