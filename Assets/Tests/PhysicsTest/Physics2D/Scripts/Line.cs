using UnityEngine;

namespace PhysicsTest
{
    public class Line : MonoBehaviour
    {
        public Transform p1;
        public Transform p2;

        private void OnDrawGizmos()
        {
            if (p1 == null || p2 == null)
            {
                return;
            }

            Gizmos.DrawLine(p1.position, p2.position);
        }
    }
}