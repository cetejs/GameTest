using System;
using UnityEngine;

namespace PhysicsTest
{
    [Serializable]
    public struct Bounds
    {
        public Vector3 center;
        public Vector3 extent;
        public Vector3 Center => center;
        public Vector3 Extent => extent;
        public Vector3 Size => extent * 2f;
        public Vector3 Min => center - extent;
        public Vector3 Max => center + extent;
    }
}