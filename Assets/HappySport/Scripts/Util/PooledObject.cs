using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public ObjectPool ObjPool { get; private set; }

    public void PooledInit(ObjectPool objPool)
    {
        ObjPool = objPool;
    }

    public void ReturnPool()
    {
        ObjPool.PushPool(this);
    }
}

