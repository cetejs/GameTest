using System.Collections.Generic;
using UnityEngine;

public class ParabolaDrawer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private ParabolaInfo fixedInfo;
    [SerializeField]
    private Vector3 origin;
    [SerializeField]
    private Vector3 euler;
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    [SerializeField]
    private List<Vector3> dirs = new List<Vector3>();

    private void Start()
    {
        BuildParabola();
        DrawParabola();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 point = points[i];
            Vector3 dir = dirs[i];
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(point, 0.1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(point, point + dir);
        }

        Gizmos.color = Color.white;
    }

    private void OnValidate()
    {
        BuildParabola();
        DrawParabola();
    }

    private void BuildParabola()
    {
        ParabolaBuildInfo info = new ParabolaBuildInfo()
        {
            fixedInfo = fixedInfo,
            origin = origin,
            forward = (Quaternion.Euler(euler) * Vector3.forward).normalized,
            points = points,
            dirs = dirs
        };

        ParabolaBuilder.Build(info);
    }

    private void DrawParabola()
    {
        if (!lineRenderer)
        {
            return;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}