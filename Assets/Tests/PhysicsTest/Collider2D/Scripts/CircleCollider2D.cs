using UnityEngine;

namespace PhysicsTest
{
    public class CircleCollider2D : Collider2D
    {
        public float Radius
        {
            get { return bounds.extent.x; }
            set { bounds.extent.x = bounds.extent.y = value; }
        }

        protected override void OnDrawGizmos()
        {
            Color color = Gizmos.color;
            Gizmos.color = IsTrigger ? Color.red : color;
            float delta = 0.1f;
            Vector3 position = transform.position + transform.rotation * bounds.Center;
            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += delta)
            {
                float x = Radius * Mathf.Cos(theta);
                float y = Radius * Mathf.Sin(theta);
                Vector3 endPoint = new Vector3(x, y, 0) + position;
                if (theta == 0)
                {
                    firstPoint = endPoint;
                }
                else
                {
                    Gizmos.DrawLine(beginPoint, endPoint);
                }

                beginPoint = endPoint;
            }

            Gizmos.DrawLine(firstPoint, beginPoint);
            Gizmos.color = color;
        }
    }
}