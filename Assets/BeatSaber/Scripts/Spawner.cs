using System.Collections.Generic;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public GameObject[] cubes;
    public Transform[] points;
    // public BeatGameManager gameManager;
    public BeatSoundManager beatManager;
    public bool isGameStart;

    //오브젝트 풀링
    private Dictionary<GameObject, ObjectPool> poolDic;


    void OnEnable()
    {
        beatManager.OnBeat += SpawnCube;
        BeatGameManager.OnGameStart += GameStart;

    }
    void OnDisable()
    {
        beatManager.OnBeat -= SpawnCube;
        BeatGameManager.OnGameStart -= GameStart;
    }
    void Start()
    {
        //딕셔너리 초기화
        poolDic = new();
        //foreach문으로 큐브 배열에 있는 큐브들 오브젝트 풀링 적용
        foreach (GameObject cube in cubes)
        {
            PooledObject pooled = cube.GetComponent<PooledObject>();

            ObjectPool pool = new(this.transform, pooled);
            poolDic.Add(cube, pool);
        }
    }
    void GameStart() =>
        isGameStart = true;

    void SpawnCube()
    {
        if (!isGameStart) return;
        int cubeIndex = Random.Range(0, cubes.Length);
        int pointIndex = Random.Range(0, points.Length);

        //오브젝트 풀링
        GameObject cubPrefab = cubes[cubeIndex];
        ObjectPool pool = poolDic[cubPrefab];

        PooledObject pooledObj = pool.PopPool();
        GameObject cube = pooledObj.gameObject;

        cube.transform.position = points[pointIndex].position;
        cube.transform.rotation = Quaternion.identity;

        // GameObject cube = Instantiate(cubes[cubeIndex], points[pointIndex].position, Quaternion.identity);

        //랜덤 방향 회전
        CutDirection randomDirection = (CutDirection)Random.Range(0, 4);
        cube.transform.rotation = GetRotCube(randomDirection);

        Movement move = cube.GetComponent<Movement>();
        if (move != null)
        {
            move.moveDirection = -transform.forward;
            move.moveDuration = beatManager.leadTime;
        }

        CubeDir noteDir = cube.GetComponent<CubeDir>();
        if (noteDir != null)
            noteDir.requiredDirection = randomDirection;
    }

    Quaternion GetRotCube(CutDirection dir)
    {
        switch (dir)
        {
            case CutDirection.Up:
                return Quaternion.Euler(0, 0, 180);
            case CutDirection.Down:
                return Quaternion.Euler(0, 0, 0);
            case CutDirection.Left:
                return Quaternion.Euler(0, 0, -90);
            case CutDirection.Right:
                return Quaternion.Euler(0, 0, 90);
            default:
                return Quaternion.identity;
        }
    }
}

