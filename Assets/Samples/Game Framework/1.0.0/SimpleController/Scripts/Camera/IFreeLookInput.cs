using UnityEngine;

namespace GameFramework
{
    public interface IFreeLookInput
    {
        public Vector2 Look { get; }

        public InputDevice InputDevice { get; }
    }
}