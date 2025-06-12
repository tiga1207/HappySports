using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    //각 볼링 핀들의 각도를 계산하여 점수 쪽으로 넘기기
    public Transform initTransform;
    void OnEnable()
    {
        Subscribe();
    }
    void OnDisable()
    {
        
    }
    void Start()
    {
        //현재 위치를 초기화 위치로 지정.
        initTransform.position = transform.position;
    }

    void Update()
    {

    }
    private void ResetPinTransform()
    {
        transform.position = initTransform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    void Subscribe()
    {
        BowlingGameManager.OnTurnReset.AddListener(ResetPinTransform);
    }
}
