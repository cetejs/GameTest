using System.Collections.Generic;
using GameFramework;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/ThrowingHub", fileName = "ThrowingHub")]
public class ThrowingHub : ScriptableObject
{
    public ParabolaInfo parabolaInfo;
    public ThrowingMoveInfo moveInfo;
    public Vector3 originOffset;
    public Vector3 eulerOffset;
    public float minDistance;
    [ObjectPoolName]
    public string throwingMover;
    [ObjectPoolName]
    public string throwingPreview;
    [ObjectPoolName]
    public string throwingSpawner;

    private readonly List<Vector3> points = new List<Vector3>();
    private readonly List<Vector3> dirs = new List<Vector3>();
    private ThrowingPreview preview;
    private Vector3 spawnEuler;
    private bool canThrowing;

    public void PreThrow(Vector3 origin, Vector3 euler)
    {
        if (!preview)
        {
            preview = ObjectPoolManager.Instance.Get<ThrowingPreview>(throwingPreview);
        }

        ParabolaBuildInfo info = new ParabolaBuildInfo()
        {
            fixedInfo = parabolaInfo,
            origin = origin + originOffset,
            forward = (Quaternion.Euler(euler + eulerOffset) * Vector3.forward).normalized,
            points = points,
            dirs = dirs
        };

        spawnEuler = new Vector3(0.0f, euler.y, 0.0f);
        float length = ParabolaBuilder.Build(info);
        canThrowing = preview.Draw(new ThrowingPreviewInfo()
        {
            points = points,
            euler = spawnEuler,
            canThrowing = minDistance <= 0.0f || length > minDistance
        });
    }

    public void StartThrow()
    {
        if (!canThrowing)
        {
            ReleasePreview();
            return;
        }

        ObjectPoolManager.Instance.Get<ThrowingMover>(throwingMover).StartMove(moveInfo, points, spawnEuler, endPoint =>
        {
            ObjectPoolManager.Instance.Get<ThrowingSpawner>(throwingSpawner).Spawn(endPoint, spawnEuler);
        });

        ReleasePreview();
    }

    public void CancelThrow()
    {
        ReleasePreview();
    }

    private void ReleasePreview()
    {
        if (preview)
        {
            preview.Release();
            preview = null;
        }
    }
}