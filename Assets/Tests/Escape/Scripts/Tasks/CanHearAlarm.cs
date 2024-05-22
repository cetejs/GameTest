using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class CanHearAlarm : IntervalConditional
    {
        [SerializeField]
        private SharedFloat radius;
        [SerializeField]
        private SharedTransform warner;
        [SerializeField]
        private SharedTransform storeResult;
        
        public override TaskStatus OnConditionalUpdate()
        {
            storeResult.Value = null;
            if (!warner.Value)
            {
                return TaskStatus.Failure;
            }

            if (Vector3.SqrMagnitude(transform.position - warner.Value.position) > radius.Value * radius.Value)
            {
                return TaskStatus.Failure;
            }

            storeResult.Value = warner.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            base.OnReset();
            radius = 0f;
            warner = null;
            storeResult = null;
        }
    }
}