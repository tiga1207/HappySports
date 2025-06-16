// 개선된 곡 상세 UI + 난이도 버튼 선택 시스템
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;

public class SongListItem : MonoBehaviour
{
    [Header("기본 UI 요소")]
    public RawImage coverImage; //커버 이미지
    public TMP_Text songNameText; //곡명
    public TMP_Text artistText; //아티스트 이름

    [Header("난이도 버튼 영역")]
    public Transform diffBtnTransform; //난이도 위치
    public GameObject diffBtnPrefab; //버튼들 프리펩

    [Header("게임 시작 버튼")]
    public Button playButton;

    private DifficultyBeatmap selectedDiff;
    private List<GameObject> createdButtons = new();

    public void Init(SongFolder folder, UnityAction<SongFolder, DifficultyBeatmap> onPlayClick)
    {
        // 텍스트 채우기
        songNameText.text = folder.info.songName;
        artistText.text = folder.info.songAuthor;

        if (folder.coverImage != null)
            coverImage.texture = folder.coverImage;

        // 난이도 버튼 초기화
        foreach (var go in createdButtons)
            Destroy(go);
        createdButtons.Clear();

        //난이도 배튼 생성
        foreach (var diff in folder.info.difficultyBeatmaps)
        {
            GameObject btnObj = Instantiate(diffBtnPrefab, diffBtnTransform);
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
            Button btn = btnObj.GetComponent<Button>();
            btnText.text = diff.difficulty;

            btn.onClick.AddListener(() => OnDifficultySelected(diff, btnObj));
            createdButtons.Add(btnObj);
        }

        // 게임 시작 버튼
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
        {
            if (selectedDiff != null)
            {
                onPlayClick?.Invoke(folder, selectedDiff);
            }
        });

        // 기본 선택 없음
        selectedDiff = null;
    }

    private void OnDifficultySelected(DifficultyBeatmap diff, GameObject selectedBtn)
    {
        selectedDiff = diff;

        // 하이라이트 처리
        foreach (var btn in createdButtons)
        {
            Image image = btn.GetComponent<Image>();

            //선택 되면 노란색 아니면 흰색
            image.color = (btn == selectedBtn) ? Color.yellow : Color.white;
        }
    }
}
