using System;
using System.Collections.Generic;
using GameFramework.Generic;
using GameFramework.ObjectPoolService;
using GameFramework.Utils;
using UnityEngine;

public class ThrowingMover : PoolObject
{
    [SerializeField] [ReadOnly]
    public ThrowingMoveInfo info;
    [SerializeField] [ReadOnly]
    private List<Vector3> movePath = new List<Vector3>();
    [SerializeField] [ReadOnly]
    private int nextIndex;
    private Action<Vector3> onMoveEnd;

    public void StartMove(ThrowingMoveInfo info, List<Vector3> movePath, Vector3 euler,Action<Vector3> onMoveEnd)
    {
        this.info = info;
        this.movePath.Clear();
        this.movePath.AddRange(movePath);
        this.onMoveEnd = onMoveEnd;
        transform.eulerAngles = euler;
        if (movePath.Count > 0)
        {
            transform.position = movePath[0];
            nextIndex = 1;
        }
    }

    private void Update()
    {
        if (nextIndex == -1)
        {
            Release();
            return;
        }

        if (nextIndex < movePath.Count)
        {
            Vector3 nowPoint = transform.position;
            Vector3 nextPoint = movePath[nextIndex];

            if (VectorUtils.SqrDistance(nowPoint, nextPoint) > 0.1f)
            {
                nowPoint = Vector3.MoveTowards(nowPoint, nextPoint, info.moveSpeed * Time.deltaTime);
                if (!info.isIgnoreCollider)
                {
                    Vector3 dir = nowPoint - transform.position;
#if UNITY_EDITOR
                    Debug.DrawLine(transform.position, nowPoint, Color.red);
#endif

                    if (Physics.Raycast(nowPoint, dir.normalized, out RaycastHit hitInfo, dir.magnitude, info.collisionMask, QueryTriggerInteraction.Ignore))
                    {
                        onMoveEnd?.Invoke(hitInfo.point);
                        nextIndex = -1;
                        return;
                    }
                }

                transform.position = nowPoint;
            }
            else
            {
                transform.position = nextPoint;
                nextIndex++;
            }
        }
        else
        {
            onMoveEnd?.Invoke(transform.position);
            nextIndex = -1;
        }
    }
}