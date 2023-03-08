using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class AutoRotate : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 eulerAngles;
        [SerializeField]
        private Space relativeTo = Space.Self;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }
        
        public override TaskStatus OnUpdate()
        {
            transform.Rotate(eulerAngles.Value * Time.deltaTime, relativeTo);
            return TaskStatus.Running;
        }
        
        public override void OnReset()
        {
            target = null;
            eulerAngles = Vector3.zero;
            relativeTo = Space.Self;
        }
    }
}
