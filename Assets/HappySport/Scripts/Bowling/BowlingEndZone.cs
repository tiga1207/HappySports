using UnityEngine;
using UnityEngine.Events;

public class BowlingEndZone : MonoBehaviour
{
    public static UnityEvent OnIsBallEntered =new();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            OnIsBallEntered?.Invoke();
        }
    }
}