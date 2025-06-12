using UnityEngine;
using UnityEngine.Events;
public class BowlingGameManager : MonoBehaviour
{
    //TODO: 싱글톤 쓸지는 고려해보자. 아직은 쓸 필요 없어보임.
    [Header("EVENT")]
    public static UnityEvent OnTurnReset = new(); //각 턴 종료시 리셋 이벤트 발행
    public static UnityEvent<int> OnScoreReturn = new(); //각 턴 종료시 리셋 이벤트 발행

    [Header("Turn Info")]
    [SerializeField] private bool timerTurn = false;
    public float turnTimer = 3f; //각 턴마다의 타이머
    public float turnTime = 3f;
    public int currentTurn = 1; // 현재 속한 턴(초기값은 1)
    public int totalTurn; //턴 수
    public bool isEndCondition = false;

    [Header("Score")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int totalScore = 0;
    // [Header("Pin")]
    // [SerializeField] private Transform pinTransform;
    // [SerializeField] private GameObject pinGrabPrefab;


    void OnEnable()
    {
        SubScribeEvents();
    }
    void OnDisable()
    {
        UnSubScribeEvents();
    }
    void Update()
    {
        if (timerTurn)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0 || isEndCondition)
            {
                IsTunrOvered();
            }
        }
    }

    private void Init()
    {
        turnTimer = turnTime;
        timerTurn = false;
        //현재 턴이 마지막 턴일 경우를 대조
        currentTurn = (currentTurn != totalTurn) ? currentTurn + 1 : 1;
    }

    //각 턴 종료 시
    public void IsTunrOvered()
    {
        //턴 종료 알림
        OnTurnReset?.Invoke();

        //TODO: 점수 계산매니저에서 점수 계산

        totalScore += currentScore;

        isEndCondition = false;

        //현재 턴이 최종턴이 아닐 경우
        if (currentTurn != totalTurn)
        {
            Init();
        }
        else //최종 턴일 경우
        {
            AllTurnOvered();
        }
    }

    //모든 턴 종료 시 메서드
    public void AllTurnOvered()
    {
        //턴 종료 알림
        OnTurnReset?.Invoke();
        Init();

        //TODO: 점수 기록할 수 있도록 UI에 최종점수 넘기기.
        OnScoreReturn?.Invoke(totalScore);

    }
    //타이머 시작
    public void StartTimer()
    {
        timerTurn = true;
    }
    public void EndCondition()
    {
        isEndCondition = true;
    }
    // public void CreatePinGroup()
    // {
    //     Instantiate(pinGrabPrefab, pinTransform);
    // }
    //이벤트 구독 관련 메서드
    void SubScribeEvents()
    {
        BowlingBall.OnPlayerThrow.AddListener(StartTimer);
        BowlingBall.OnBallStopped.AddListener(EndCondition);
        BowlingEndZone.OnIsBallEntered.AddListener(EndCondition);
    }
    void UnSubScribeEvents()
    {
        BowlingBall.OnPlayerThrow.RemoveListener(StartTimer);
        BowlingBall.OnBallStopped.RemoveListener(EndCondition);
        BowlingEndZone.OnIsBallEntered.RemoveListener(EndCondition);
    }
}