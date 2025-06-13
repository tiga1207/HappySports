using UnityEngine;

public enum CutDirection
{
    Up,
    Down,
    Left,
    Right
}

public class CubeDir : MonoBehaviour
{
    public CutDirection requiredDirection;

    public Vector3 GetSwingDir()
    {
        // 휘두른 방향은 평면 위에서 어느 쪽으로 움직였는지를 나타냄
        switch (requiredDirection)
        {
            case CutDirection.Up: return Vector3.up;
            case CutDirection.Down: return Vector3.down;
            case CutDirection.Left: return Vector3.left;
            case CutDirection.Right: return Vector3.right;
            default: return Vector3.down;
        }
    }
}