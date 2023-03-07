using UnityEngine;

public static class ParabolaBuilder
{
    public static float Build(ParabolaBuildInfo info)
    {
        info.points.Clear();
        info.dirs.Clear();
        info.points.Add(info.origin);
        info.dirs.Add(info.forward);
        float length = 0f;
        Vector3 point = info.points[0];
        ParabolaInfo fixedInfo = info.fixedInfo;
        for (int i = 1; i < fixedInfo.maxPointNum; i++)
        {
            float t = i * fixedInfo.timeInterval;
            Vector3 v0 = info.forward * fixedInfo.initialSpeed;
            Vector3 nextPoint = info.origin + v0 * t - 0.5f * Vector3.up * fixedInfo.gravity * t * t;
            Vector3 dir = nextPoint - point;
            float distance = dir.magnitude;
#if UNITY_EDITOR
            Debug.DrawLine(point, point + dir, i % 2 == 0 ? Color.blue : Color.red);
#endif
            if (Physics.Raycast(point, dir.normalized, out RaycastHit hitInfo, distance, fixedInfo.collisionMask, QueryTriggerInteraction.Ignore))
            {
                info.points.Add(hitInfo.point);
                info.dirs.Add(dir.normalized);
                break;
            }

            if (length > fixedInfo.maxLength)
            {
                break;
            }
            
            point = nextPoint;
            length += distance;
            info.points.Add(point);
            info.dirs.Add(dir.normalized);
        }

        return length;
    }
}