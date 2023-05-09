using UnityEngine;

namespace PhysicsTest
{
    public static class Collider2DUtils
    {
        public static bool IsIntersect(Collider2D c1, Collider2D c2)
        {
            BoxCollider2D box1 = c1 as BoxCollider2D;
            BoxCollider2D box2 = c2 as BoxCollider2D;
            CircleCollider2D circle1 = c1 as CircleCollider2D;
            CircleCollider2D circle2 = c2 as CircleCollider2D;

            if (box1 && box2)
            {
                if (c1.transform.rotation == Quaternion.identity &&
                    c2.transform.rotation == Quaternion.identity)
                {
                    return BoxBoxAABBIntersect(box1, box2);
                }

                return BoxBoxOBBIntersect(box1, box2);
            }

            if (circle1 && circle2)
            {
                return CircleCircleAABBIntersect(circle1, circle2);
            }

            if (box1 && circle2)
            {
                if (box1.transform.rotation == Quaternion.identity)
                {
                    return BoxCircleAABBIntersectB(box1, circle2);
                }

                return BoxCircleOBBIntersectB(box1, circle2);
            }

            if (box2 && circle1)
            {
                if (box2.transform.rotation == Quaternion.identity)
                {
                    return BoxCircleAABBIntersectB(box2, circle1);
                }

                return BoxCircleOBBIntersectB(box2, circle1);
            }

            return false;
        }

        public static bool BoxBoxAABBIntersect(BoxCollider2D c1, BoxCollider2D c2)
        {
            Vector3 p1 = c1.transform.position;
            Vector3 p2 = c2.transform.position;
            bool xCol = p1.x + c1.bounds.Max.x >= p2.x + c2.bounds.Min.x &&
                        p1.x + c1.bounds.Min.x <= p2.x + c2.bounds.Max.x;

            if (!xCol) return false;

            bool yCol = p1.y + c1.bounds.Max.y >= p2.y + c2.bounds.Min.y &&
                        p1.y + c1.bounds.Min.y <= p2.y + c2.bounds.Max.y;

            return yCol;
        }

        public static bool CircleCircleAABBIntersect(CircleCollider2D c1, CircleCollider2D c2)
        {
            Vector3 p1 = c1.transform.position + c1.transform.rotation * c1.bounds.Center;
            Vector3 p2 = c2.transform.position + c2.transform.rotation * c2.bounds.Center;
            float distance = c1.Radius + c2.Radius;

            return Vector3.SqrMagnitude(p1 - p2) <= distance * distance;
        }

        public static bool BoxCircleAABBIntersectA(BoxCollider2D c1, CircleCollider2D c2)
        {
            Vector3 p1 = c1.transform.position + c1.bounds.Center;
            Vector3 p2 = c2.transform.position + c2.transform.rotation * c2.bounds.Center;

            Vector3 clamp = Vector3Utils.Clamp(p2 - p1, -c1.bounds.Extent, c1.bounds.Extent);
            Vector3 closest = p1 - p2 + clamp;
            return Vector3.SqrMagnitude(closest) <= c2.Radius * c2.Radius;
        }

        public static bool BoxCircleAABBIntersectB(BoxCollider2D c1, CircleCollider2D c2)
        {
            Vector3 p1 = c1.transform.position + c1.bounds.Center;
            Vector3 p2 = c2.transform.position + c2.transform.rotation * c2.bounds.Center;

            Vector3 v = Vector3Utils.Abs(p2 - p1);
            Vector3 h = c1.bounds.Extent;
            Vector3 u = Vector3Utils.Max(v - h, Vector3.zero);
            return Vector3.SqrMagnitude(u) <= c2.Radius * c2.Radius;
        }

        public static bool BoxBoxOBBIntersect(BoxCollider2D c1, BoxCollider2D c2)
        {
            Quaternion r1 = c1.transform.rotation;
            Quaternion r2 = c2.transform.rotation;
            Vector3 p1 = c1.transform.position + r1 * c1.bounds.Center;
            Vector3 p2 = c2.transform.position + r2 * c2.bounds.Center;
            Vector3 ax1 = r1 * Vector3.right;
            Vector3 ax2 = r2 * Vector3.right;
            Vector3 ay1 = r1 * Vector3.up;
            Vector3 ay2 = r2 * Vector3.up;
            Vector3 v = p1 - p2;

            Vector3 x1 = ax1 * c1.bounds.Extent.x;
            Vector3 x2 = ax2 * c2.bounds.Extent.x;
            Vector3 y1 = ay1 * c1.bounds.Extent.y;
            Vector3 y2 = ay2 * c2.bounds.Extent.y;

            return ProjectionOverlap(x1, x2, y1, y2, v, ax1) &&
                   ProjectionOverlap(x1, x2, y1, y2, v, ax2) &&
                   ProjectionOverlap(x1, x2, y1, y2, v, ay1) &&
                   ProjectionOverlap(x1, x2, y1, y2, v, ay2);
        }

        private static bool ProjectionOverlap(Vector3 x1, Vector3 x2, Vector3 y1, Vector3 y2, Vector3 v, Vector3 axis)
        {
            float lx1 = Vector3.Dot(x1, axis);
            float lx2 = Vector3.Dot(x2, axis);
            float ly1 = Vector3.Dot(y1, axis);
            float ly2 = Vector3.Dot(y2, axis);
            float lv = Vector3.Dot(v, axis);
            return Mathf.Abs(lv) <= Mathf.Abs(lx1) + Mathf.Abs(lx2) + Mathf.Abs(ly1) + Mathf.Abs(ly2);
        }

        public static bool BoxCircleOBBIntersectA(BoxCollider2D c1, CircleCollider2D c2)
        {
            Vector3 p1 = c1.transform.position + c1.transform.rotation * c1.bounds.Center;
            Vector3 p2 = c2.transform.position + c2.transform.rotation * c2.bounds.Center;
            p2 = (Quaternion.Inverse(c1.transform.rotation) * (p2 - p1)) + p1;

            Vector3 clamp = Vector3Utils.Clamp(p2 - p1, -c1.bounds.Extent, c1.bounds.Extent);
            Vector3 closest = p1 - p2 + clamp;
            return Vector3.SqrMagnitude(closest) <= c2.Radius * c2.Radius;
        }

        public static bool BoxCircleOBBIntersectB(BoxCollider2D c1, CircleCollider2D c2)
        {
            Vector3 p1 = c1.transform.position + c1.transform.rotation * c1.bounds.Center;
            Vector3 p2 = c2.transform.position + c2.transform.rotation * c2.bounds.Center;
            p2 = Quaternion.Inverse(c1.transform.rotation) * (p2 - p1) + p1;

            Vector3 v = Vector3Utils.Abs(p2 - p1);
            Vector3 h = c1.bounds.Extent;
            Vector3 u = Vector3Utils.Max(v - h, Vector3.zero);
            return Vector3.SqrMagnitude(u) <= c2.Radius * c2.Radius;
        }
    }
}