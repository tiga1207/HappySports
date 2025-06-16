using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class NoteSpawner : MonoBehaviour
{
    [Header("Beatmap Data")]
    public TextAsset beatMapJson;

    [Header("Note Prefabs")]
    public GameObject leftCubePrefab;  // 왼손용 (블루)
    public GameObject rightCubePrefab; // 오른손용 (레드)

    [Header("Audio")]
    public AudioSource musicSource;

    private BeatMapData beatMap;

    private float cubeSpacing = 0.7f; //큐브 x축 간격 변수

    private float bpm;
    private Dictionary<GameObject, ObjectPool> poolDic = new();

    void Start()
    {
        poolDic[leftCubePrefab] = new ObjectPool(this.transform, leftCubePrefab.GetComponent<PooledObject>());
        poolDic[rightCubePrefab] = new ObjectPool(this.transform, rightCubePrefab.GetComponent<PooledObject>());
    }

    public void LoadBeatMapFromPath(string path, float bpm)
    {
        this.bpm = bpm;
        beatMapJson = Resources.Load<TextAsset>(path);

        if (beatMapJson == null)
        {
            Debug.Log($"맵 파일을 찾을 수 없습니다: {path}");
            return;
        }

        Debug.Log($"맵 로딩 완료: {path}");

        // 실제 데이터 파싱
        beatMap = JsonConvert.DeserializeObject<BeatMapData>(beatMapJson.text);


        musicSource.Play();
        // 노트 생성 코루틴 시작
        StartCoroutine(SpawnNotesCoroutine());

    }

    IEnumerator SpawnNotesCoroutine()
    {
        float startTime = Time.time;

        foreach (var note in beatMap._notes)
        {
            float spawnTime = note._time * 60f / bpm;
            float targetTime = startTime + spawnTime;
            float delay = targetTime - Time.time;

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            SpawnNote(note);
        }

    }

    void SpawnNote(NoteData note)
    {

        Vector3 spawnPos = new Vector3(
            (note._lineIndex - 1.5f) * cubeSpacing,
            note._lineLayer + this.transform.position.y,
            this.transform.position.z
            );

        GameObject prefab = note._type == 0 ? leftCubePrefab : rightCubePrefab;

        // GameObject cube = Instantiate(prefab, spawnPos, Quaternion.identity);

        //오브젝트 풀링 생성 방식
        ObjectPool pool = poolDic[prefab];

        GameObject cube = pool.PopPool().gameObject;
        cube.transform.position = spawnPos;
        cube.transform.rotation = Quaternion.identity;


        CubeDir cubeDir = cube.GetComponent<CubeDir>();
        if (cubeDir != null)
        {
            cubeDir.requiredDirection = ConvertDirection(note._cutDirection);
            cubeDir.ApplyRotation();
        }
        else
            Debug.Log("CubeDir 컴포넌트가 없습니다.");
    }

    CutDirection ConvertDirection(int dir)
    {
        // 비트세이버 방식 -> CutDirection 매핑
        return dir switch
        {
            0 => CutDirection.Up,
            1 => CutDirection.Down,
            2 => CutDirection.Left,
            3 => CutDirection.Right,
            4 => CutDirection.Up,    // 대각선은 임시 처리
            5 => CutDirection.Up,
            6 => CutDirection.Down,
            7 => CutDirection.Down,
            _ => CutDirection.Down,
        };
    }
}
