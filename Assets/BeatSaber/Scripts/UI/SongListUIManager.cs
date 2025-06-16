using Unity.VisualScripting;
using UnityEngine;

public class SongListUIManager : MonoBehaviour
{
    // 곡 정보 들어있는 프리팹(SongListItem)
    public GameObject songItemPrefab;

    // 곡 데이터 로더
    public SongLoader songLoader;

    // Scroll View의 Content 오브젝트
    public Transform contentParent;

    void Start()
    {
        UpdateSongList();
    }

    void UpdateSongList()
    {
        //스크롤뷰 컨텐츠 내부 초기화
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        //업데이트
        foreach (var folder in songLoader.allSongs)
        {
            GameObject item = Instantiate(songItemPrefab, contentParent);
            item.GetComponent<SongListItem>().Init(folder, OnSongSelected);
        }
    }

    // 선택된 곡과 난이도로 게임 시작
    void OnSongSelected(SongFolder folder, DifficultyBeatmap selected)
    {
        GameManager.Instance.StartGame(folder, selected.difficulty);
        // 캔버스 비활성화
        this.gameObject.SetActive(false);
    }
}
