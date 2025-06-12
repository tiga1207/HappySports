using UnityEngine;
using UnityEngine.Events;
public class BowlingGameManager : MonoBehaviour
{
    [Header("Turn Info")]
    public UnityEvent TurnReset; //각 턴 종료시 리셋 이벤트 발행
    public float turnTimer; //각 턴마다의 타이머
    public int currentTurn = 1; // 현재 속한 턴(초기값은 1)
    public int totalTurn; //턴 수

    public void IsTunrOvered()
    {

    }
    public void AllTurnOvered()
    {

    }
}