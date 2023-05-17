using UnityEngine;

namespace PhysicsTest
{
    public static class Physics2DUtils
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

        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, out HitInfo2D hit, Collider2D col)
        {
            hit = new HitInfo2D();
            BoxCollider2D box = col as BoxCollider2D;
            CircleCollider2D circle = col as CircleCollider2D;
            if (box != null)
            {
                return RaycastBox(origin, direction, maxDistance, out hit, box);
            }

            if (circle != null)
            {
                return RaycastCircle(origin, direction, maxDistance, out hit, circle);
            }

            return false;
        }
        
        public static bool RaycastBox(Vector3 origin, Vector3 direction, float maxDistance, out HitInfo2D hit, BoxCollider2D col)
        {
            Vector3 start = origin;
            Vector3 end = origin + direction.normalized * maxDistance;

            hit = new HitInfo2D();
            Vector3 p0 = col.transform.position;
            Vector3 p1 = p0 + col.transform.rotation * (col.bounds.Center + col.bounds.Extent);
            Vector3 p2 = p0 + col.transform.rotation * (col.bounds.Center + new Vector3(-col.bounds.Extent.x, col.bounds.Extent.y));
            Vector3 p3 = p0 + col.transform.rotation * (col.bounds.Center - col.bounds.Extent);
            Vector3 p4 = p0 + col.transform.rotation * (col.bounds.Center + new Vector3(col.bounds.Extent.x, -col.bounds.Extent.y));

            int count = 0;
            Vector3 point;
            if (Linecast(start, end, p1, p2, out point))
            {
                count++;
                hit.point = point;
                hit.normal = Quaternion.Euler(0, 0, 90) * (p1 - p2).normalized;
            }

            if (Linecast(start, end, p2, p3, out point))
            {
                if (count++ > 0)
                {
                    float d1 = Vector3.SqrMagnitude(start - hit.point);
                    float d2 = Vector3.SqrMagnitude(start - point);
                    if (d1 > d2)
                    {
                        hit.point = point;
                        hit.normal = Quaternion.Euler(0, 0, 90) * (p2 - p3).normalized;
                    }

                    return true;
                }

                hit.point = point;
                hit.normal = Quaternion.Euler(0, 0, 90) * (p2 - p3).normalized;
            }

            if (Linecast(start, end, p3, p4, out point))
            {
                if (count++ > 0)
                {
                    float d1 = Vector3.SqrMagnitude(start - hit.point);
                    float d2 = Vector3.SqrMagnitude(start - point);
                    if (d1 > d2)
                    {
                        hit.point = point;
                        hit.normal = Quaternion.Euler(0, 0, 90) * (p3 - p4).normalized;
                    }

                    return true;
                }

                hit.point = point;
                hit.normal = Quaternion.Euler(0, 0, 90) * (p3 - p4).normalized;
            }

            if (Linecast(start, end, p4, p1, out point))
            {
                if (count++ > 0)
                {
                    float d1 = Vector3.SqrMagnitude(start - hit.point);
                    float d2 = Vector3.SqrMagnitude(start - point);
                    if (d1 > d2)
                    {
                        hit.point = point;
                        hit.normal = Quaternion.Euler(0, 0, 90) * (p4 - p1).normalized;
                    }

                    return true;
                }

                hit.point = point;
                hit.normal = Quaternion.Euler(0, 0, 90) * (p4 - p1).normalized;
            }

            if (count > 0)
            {
                return true;
            }

            return false;
        }

        public static bool RaycastCircle(Vector3 origin, Vector3 direction, float maxDistance, out HitInfo2D hit, CircleCollider2D col)
        {
            hit = new HitInfo2D();
            Vector3 start = origin;
            Vector3 n = direction.normalized;
            float d = maxDistance;
            Vector3 end = start + n * d;
            float r = col.Radius;
            float r2 = r * r;
            Vector3 c = col.transform.position + col.bounds.Center;
            bool startInCircle = Vector3.SqrMagnitude(c - start) <= r2;
            bool endInCircle = Vector3.SqrMagnitude(c - end) <= r2;
            if (startInCircle && endInCircle)
            {
                return false;
            }

            if (!startInCircle && !endInCircle)
            {
                float p = Vector3.Dot(c - start, n);
                if (p < 0 || p > d)
                {
                    return false;
                }
            }

            if (startInCircle)
            {
                start = end;
                n = -n;
            }

            Vector3 e = c - start;
            Vector3 a = Vector3.Dot(e, n) * n;
            float a2 = Vector3.Dot(a, a);
            float e2 = Vector3.Dot(e, e);
            float d2 = e2 - a2;
            if (d2 > r2)
            {
                return false;
            }

            Vector3 f = Mathf.Sqrt(r2 - d2) * n;
            Vector3 t = a - f;
            hit.point = start + t;
            hit.normal = (t - e).normalized;
            hit.other = col;
            return true;
        }

        public static int Linecast(Vector3 start, Vector3 end, HitInfo2D[] result, Collider2D col)
        {
            BoxCollider2D box = col as BoxCollider2D;
            CircleCollider2D circle = col as CircleCollider2D;
            if (box != null)
            {
                return LinecastBox(start, end, result, box);
            }

            if (circle != null)
            {
                return LinecastCircle(start, end, result, circle);
            }

            return 0;
        }

        public static int LinecastBox(Vector3 start, Vector3 end, HitInfo2D[] result, BoxCollider2D col)
        {
            if (result == null || result.Length == 0)
            {
                return 0;
            }
            
            HitInfo2D hit1 = new HitInfo2D();
            HitInfo2D hit2 = new HitInfo2D();
            Vector3 p0 = col.transform.position;
            Vector3 p1 = p0 + col.transform.rotation * (col.bounds.Center + col.bounds.Extent);
            Vector3 p2 = p0 + col.transform.rotation * (col.bounds.Center + new Vector3(-col.bounds.Extent.x, col.bounds.Extent.y));
            Vector3 p3 = p0 + col.transform.rotation * (col.bounds.Center - col.bounds.Extent);
            Vector3 p4 = p0 + col.transform.rotation * (col.bounds.Center + new Vector3(col.bounds.Extent.x, -col.bounds.Extent.y));

            int count = 0;
            Vector3 point;
            if (Linecast(start, end, p1, p2, out point))
            {
                count++;
                hit1.point = point;
                hit1.normal = Quaternion.Euler(0, 0, 90) * (p1 - p2).normalized;
                result[0] = hit1;
            }

            if (Linecast(start, end, p2, p3, out point))
            {
                if (count++ > 0)
                {
                    hit2.point = point;
                    hit2.normal = Quaternion.Euler(0, 0, 90) * (p2 - p3).normalized;
                    result[1] = hit2;
                    return count;
                }

                hit1.point = point;
                hit1.normal = Quaternion.Euler(0, 0, 90) * (p2 - p3).normalized;
                result[0] = hit1;
            }

            if (Linecast(start, end, p3, p4, out point))
            {
                if (count++ > 0)
                {
                    hit2.point = point;
                    hit2.normal = Quaternion.Euler(0, 0, 90) * (p3 - p4).normalized;
                    result[1] = hit2;
                    return count;
                }

                hit1.point = point;
                hit1.normal = Quaternion.Euler(0, 0, 90) * (p3 - p4).normalized;
                result[0] = hit1;
            }

            if (Linecast(start, end, p4, p1, out point))
            {
                if (count++ > 0)
                {
                    hit2.point = point;
                    hit2.normal = Quaternion.Euler(0, 0, 90) * (p4 - p1).normalized;
                    result[1] = hit2;
                    return count;
                }

                hit1.point = point;
                hit1.normal = Quaternion.Euler(0, 0, 90) * (p4 - p1).normalized;
                result[0] = hit1;
            }

            return count;
        }

        public static int LinecastCircle(Vector3 start, Vector3 end, HitInfo2D[] result, CircleCollider2D col)
        {
            if (result == null || result.Length == 0)
            {
                return 0;
            }

            Vector3 n = (end - start).normalized;;
            float d = (end - start).magnitude;
            float r = col.Radius;
            float r2 = r * r;
            Vector3 c = col.transform.position + col.bounds.Center;
            bool startInCircle = Vector3.SqrMagnitude(c - start) <= r2;
            bool endInCircle = Vector3.SqrMagnitude(c - end) <= r2;
            if (startInCircle && endInCircle)
            {
                return 0;
            }
            
            if (!startInCircle && !endInCircle)
            {
                float p = Vector3.Dot(c - start, n);
                if (p < 0 || p > d)
                {
                    return 0;
                }
            }
            Vector3 e = c - start;
            Vector3 a = Vector3.Dot(e, n) * n;
            float a2 = Vector3.Dot(a, a);
            float e2 = Vector3.Dot(e, e);
            float d2 = e2 - a2;
            if (d2 > r2)
            {
                return 0;
            }
            
            HitInfo2D hit1 = new HitInfo2D();
            HitInfo2D hit2 = new HitInfo2D();
            if (!startInCircle)
            {
                Vector3 f = Mathf.Sqrt(r2 - d2) * n;
                Vector3 t = a - f;
                hit1.point = start + t;
                hit1.normal = (t - e).normalized;
                hit1.other = col;
            }

            if (!endInCircle)
            {
                n = -n;
                e = c - end;
                a = Vector3.Dot(e, n) * n;
                Vector3 f = Mathf.Sqrt(r2 - d2) * n;
                Vector3 t = a - f;
                hit2.point = end + t;
                hit2.normal = (t - e).normalized;
                hit2.other = col;
            }

            if (startInCircle)
            {
                result[0] = hit2;
                return 1;
            }

            if (endInCircle)
            {
                result[0] = hit1;
                return 1;
            }

            if (result.Length == 1)
            {
                result[0] = hit1;
                return 1;
            }

            result[0] = hit1;
            result[1] = hit2;
            return 2;
        }

        public static bool Linecast(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 hit)
        {
            hit = Vector3.zero;

            Vector3 ab = b - a;
            Vector3 cd = d - c;
            Vector3 ca = a - c;

            if (Vector3.Cross(ab, cd).sqrMagnitude <= 1e-5)
            {
                return false;
            }

            if (Mathf.Min(a.x, b.x) > Mathf.Max(c.x, d.x) ||
                Mathf.Min(c.x, d.x) > Mathf.Max(a.x, b.x) ||
                Mathf.Min(a.y, b.y) > Mathf.Max(c.y, d.y) ||
                Mathf.Min(c.y, d.y) > Mathf.Max(a.y, b.y))
            {
                return false;
            }

            Vector3 cb = b - c;
            Vector3 ad = d - a;

            if (Vector3.Dot(Vector3.Cross(ca, cd), Vector3.Cross(cd, cb)) > 0 &&
                Vector3.Dot(Vector3.Cross(-ca, ab), Vector3.Cross(ab, ad)) > 0)
            {
                Vector3 v1 = Vector3.Cross(ca, cd);
                Vector3 v2 = Vector3.Cross(cd, ab);
                float ratio = Vector3.Dot(v1, v2) / v2.sqrMagnitude;
                //float ratio = Mathf.Sqrt(v1.sqrMagnitude / v2.sqrMagnitude);
                hit = a + (b - a) * ratio;
                return true;
            }

            return false;
        }
    }
}