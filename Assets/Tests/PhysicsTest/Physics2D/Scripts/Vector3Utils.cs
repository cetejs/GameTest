using UnityEngine;

namespace PhysicsTest
{
    public static class Vector3Utils
    {
        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            Vector3 v;
            v.x = Mathf.Max(a.x, b.x);
            v.y = Mathf.Max(a.y, b.y);
            v.z = Mathf.Max(a.z, b.z);
            return v;
        }

        public static Vector3 Clamp(Vector3 v, Vector3 a, Vector3 b)
        {
            v.x = Mathf.Clamp(v.x, a.x, b.x);
            v.y = Mathf.Clamp(v.y, a.y, b.y);
            v.z = Mathf.Clamp(v.z, a.z, b.z);
            return v;
        }
        
        public static Vector3 Abs(Vector3 v)
        {
            v.x = Mathf.Abs(v.x);
            v.y = Mathf.Abs(v.y);
            v.z = Mathf.Abs(v.z);
            return v;
        }
    }
}