using System;
using TMPro;
using UnityEngine;
public class BeatScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    //총 점수
    [SerializeField] private int totalScroe = 0;
    // [SerializeField] private int preTotalScroe = 0;

    //점수
    [SerializeField] private int point = 1;

    void OnEnable()
    {
        Saber.OnScoreUp += PlusScore;
        BeatGameManager.OnGameStart += Init;
    }
    void OnDisable()
    {
        Saber.OnScoreUp -= PlusScore;
        BeatGameManager.OnGameStart -= Init;
    }
    void Start()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        scoreText.text = $"{totalScroe} 점";
    }
    void PlusScore()
    {
        totalScroe += point;
        UpdateUI();
    }

    void Init()
    {
        totalScroe = 0;
        UpdateUI();
    }
    // void StoreScore()
    // {
    //     //전에 저장한 총점이 현재 총점과 똑같으면 저장할 필요 X
    //     if (preTotalScroe == totalScroe) return;

    //     //만약 전에 저장한 총점이 없다면(첫 시도)
    //     if (preTotalScroe == 0)
    //         preTotalScroe = totalScroe;
    //     //TODO: 데이터 베이스에 저장하는 로직
    //     //현재 총점이 전의 총점보다 크다면.
    //     else if (preTotalScroe <= totalScroe)
    //     {
    //         preTotalScroe = totalScroe;
    //         //TODO: 데이터 베이스에 저장하는 로직
    //     }
    // }
}