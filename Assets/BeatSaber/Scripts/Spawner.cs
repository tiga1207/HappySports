using UnityEngine;
public class Spawner : MonoBehaviour
{
    public GameObject[] cubes;
    public Transform[] points;
    public BeatGameManager gameManager;
    public BeatSoundManager beatManager;
    public bool isGameStart;


    void OnEnable()
    {
        beatManager.OnBeat += SpawnCube;
        gameManager.OnGameStart += GameStart;

    }
    void OnDisable()
    {
        beatManager.OnBeat -= SpawnCube;
        gameManager.OnGameStart -= GameStart;
    }
    void GameStart() =>
        isGameStart = true;

    void SpawnCube()
    {
        if (!isGameStart) return;
        int cubeIndex = Random.Range(0, cubes.Length);
        int pointIndex = Random.Range(0, points.Length);

        //TODO: 오브젝트 풀링으로 대여 반납 구조로 변경하기
        GameObject cube = Instantiate(cubes[cubeIndex], points[pointIndex].position, Quaternion.identity);

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

