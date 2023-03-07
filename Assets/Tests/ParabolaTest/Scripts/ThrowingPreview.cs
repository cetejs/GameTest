using GameFramework.ObjectPoolService;
using UnityEngine;

public class ThrowingPreview : PoolObject
{
    [SerializeField]
    protected LineRenderer lineRenderer;
    [SerializeField]
    protected Material[] colorMats;
    private int lineMatIndex;

    protected override void OnSleep()
    {
        lineRenderer.positionCount = 0;
    }

    public virtual bool Draw(ThrowingPreviewInfo info)
    {
        lineRenderer.positionCount = info.points.Count;
        lineRenderer.SetPositions(info.points.ToArray());
        int index = info.canThrowing ? 0 : 1;
        if (lineMatIndex != index)
        {
            lineMatIndex = index;
            lineRenderer.material = colorMats[index];
        }

        return info.canThrowing;
    }
}