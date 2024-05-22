using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class ReplaceMaterial : Action
    {
        [SerializeField]
        private MeshRenderer renderer;
        [SerializeField]
        private Material material;

        public override TaskStatus OnUpdate()
        {
            renderer.material = material;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            renderer = null;
            material = null;
        }
    }
}