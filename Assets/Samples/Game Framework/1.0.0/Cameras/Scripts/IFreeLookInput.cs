using UnityEngine;

namespace GameFramework
{
    public interface IFreeLookInput
    {
        public Vector2 Look { get; }
        public bool IsCurrentDeviceMouse { get; }
    }
}