using UnityEngine;

namespace GameFramework.Samples.SimpleController
{
    public interface IFreeLookInput
    {
        public Vector2 Look { get; }

        public InputDevice InputDevice { get; }
    }
}