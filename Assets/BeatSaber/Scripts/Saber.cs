using UnityEngine;
using EzySlice;
using System;

public class Saber : MonoBehaviour
{
    public LayerMask sliceableLayer;
    public Material cutMaterial;
    private Vector3 previousPos;
    [SerializeField] private float acceptAngle = 45f;
    //난이도 변경로직 추가 시 해당 값 변경하면 될 듯
    public float AcceptAngle => acceptAngle;
    public static event Action OnScoreUp;

    void Update()
    {
        Vector3 swingDir = (transform.position - previousPos).normalized;
        RaycastHit hit;

        if (Physics.Raycast(previousPos, swingDir, out hit, Vector3.Distance(previousPos, transform.position), sliceableLayer))
        {

            CubeDir note = hit.transform.GetComponent<CubeDir>();

            if (note != null)
            {
                if (Vector3.Angle(swingDir, note.GetSwingDir()) < acceptAngle)
                {
                    // 카메라(플레이어가 바라보는 방향 기준으로 절단면 계산)
                    Transform cam = Camera.main.transform;
                    Vector3 cutNormal = Vector3.Cross(swingDir, cam.forward).normalized;

                    SlicedHull hull = hit.transform.gameObject.Slice(hit.point, cutNormal, cutMaterial);
                    //TODO: 점수 증가 이벤트 호출
                    OnScoreUp?.Invoke();

                    if (hull != null)
                    {
                        GameObject upper = hull.CreateUpperHull(hit.transform.gameObject, cutMaterial);
                        GameObject lower = hull.CreateLowerHull(hit.transform.gameObject, cutMaterial);

                        upper.AddComponent<MeshCollider>().convex = true;
                        upper.AddComponent<Rigidbody>();

                        lower.AddComponent<MeshCollider>().convex = true;
                        lower.AddComponent<Rigidbody>();

                        //짤린 오브젝트 파괴
                        //TODO: 오브젝트풀링 반납구조로 변경 예정정 
                        Destroy(hit.transform.gameObject);

                        //3초뒤 짤린 오브젝트 파괴
                        Destroy(upper, 3f);
                        Destroy(lower, 3f);
                    }
                }
            }
        }

        previousPos = transform.position;
    }



}
