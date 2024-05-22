using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class AutoRotate : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 eulerAngles;
        [SerializeField]
        private SharedVector2 randomRange;
        [SerializeField]
        private Space relativeTo = Space.Self;
        private Vector3 rotateAngles;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override void OnStart()
        {
            base.OnStart();
            rotateAngles = eulerAngles.Value;
            if (randomRange.Value != Vector2.zero)
            {
                if (rotateAngles.x != 0)
                {
                    rotateAngles.x += Random.Range(randomRange.Value.x, randomRange.Value.y);
                }
                
                if (rotateAngles.y != 0)
                {
                    rotateAngles.y += Random.Range(randomRange.Value.x, randomRange.Value.y);
                }
                
                if (rotateAngles.z != 0)
                {
                    rotateAngles.z += Random.Range(randomRange.Value.x, randomRange.Value.y);
                }
            }

        }

        public override TaskStatus OnUpdate()
        {
            Target.Rotate(rotateAngles * Time.deltaTime, relativeTo);
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
