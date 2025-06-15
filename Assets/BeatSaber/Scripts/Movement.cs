using UnityEngine;
public class Movement : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.back;
    public float moveDuration = 1.7f; // 큐브가 도착하기까지 걸리는 시간
    private float moveDistance=15f - 1.25f; // 플레이어까지 거리 - 원하는 타격 위치도 계산한 값.
    //큐브 속도
    private float moveSpeed;
    //난이도에 따른 속도 관리
    private float destoryTime;
    private PooledObject pooledObject;

    void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }
    void OnEnable()
    {
        destoryTime = 0f;
    }

    void Start()
    {
        moveSpeed = moveDistance / moveDuration ;
    }

    void Update()
    {
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        destoryTime += Time.deltaTime;

        if (destoryTime > moveDuration + 1f)
            // Destroy(gameObject);
            pooledObject?.ReturnPool();
    }
}
