using UnityEngine;

namespace PhysicsTest
{
    public abstract class Collider2D : MonoBehaviour
    {
        public Bounds bounds;

        public Bounds Bounds => bounds;
        public Vector2 Offset => bounds.Extent;

        public Vector3 Center
        {
            get
            {
                return transform.position + transform.rotation * bounds.Center;
            }
        }

        public bool IsTrigger { get; set; }

        public HitInfo2D HitInfo { get; set; }

        protected virtual void OnDrawGizmos()
        {
            Color color = Gizmos.color;
            Gizmos.color = IsTrigger ? Color.red : color;
            Vector3 p0 = transform.position;
            Vector3 p1 = transform.rotation * (bounds.Center + bounds.Extent);
            Vector3 p2 = transform.rotation * (bounds.Center + new Vector3(-bounds.Extent.x, bounds.Extent.y));
            Vector3 p3 = transform.rotation * (bounds.Center - bounds.Extent);
            Vector3 p4 = transform.rotation * (bounds.Center + new Vector3(bounds.Extent.x, -bounds.Extent.y));
            Gizmos.DrawLine(p0 + p1, p0 + p2);
            Gizmos.DrawLine(p0 + p2, p0 + p3);
            Gizmos.DrawLine(p0 + p3, p0 + p4);
            Gizmos.DrawLine(p0 + p4, p0 + p1);
            Gizmos.color = color;
        }
    }
}