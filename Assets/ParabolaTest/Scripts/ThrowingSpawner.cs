using GameFramework.ObjectPoolService;
using UnityEngine;

public class ThrowingSpawner : PoolObject
{
    public virtual void Spawn(Vector3 point, Vector3 euler)
    {
        transform.position = point;
        transform.eulerAngles = euler;
    }
}