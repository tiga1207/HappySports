using UnityEngine;
public class Movement : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.back;
    public float moveDuration = 1.5f; // 큐브가 도착하기까지 걸리는 시간
    public float moveDistance = 5f;     // 플레이어까지 거리

    private float moveSpeed;
    private float destoryTime;

    void Start()
    {
        moveSpeed = moveDistance / moveDuration ;
    }

    void Update()
    {
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        destoryTime += Time.deltaTime;

        if (destoryTime > moveDuration + 1f)
            Destroy(gameObject);
    }
}
