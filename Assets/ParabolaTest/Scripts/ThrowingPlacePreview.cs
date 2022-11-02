using UnityEngine;

public class ThrowingPlacePreview : ThrowingPreview
{
    [SerializeField]
    private Transform space;
    [SerializeField]
    private Renderer render;
    [SerializeField]
    private BoxCollider boxCollider;
    private int matIndex;

    public override bool Draw(ThrowingPreviewInfo info)
    {
        lineRenderer.positionCount = info.points.Count;
        lineRenderer.SetPositions(info.points.ToArray());
        space.position = info.points[info.points.Count - 1];
        space.eulerAngles = info.euler;
        bool isCollide = Physics.CheckBox(boxCollider.bounds.center + boxCollider.center, boxCollider.size / 2.0f, space.rotation);
        bool canThrowing = info.canThrowing && !isCollide;
        int index = canThrowing ? 0 : 1;
        if (matIndex != index)
        {
            matIndex = index;
            render.material = colorMats[index];
            lineRenderer.material = colorMats[index];
        }

        return canThrowing;
    }
}