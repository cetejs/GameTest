using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class StoreTarget : Action
    {
        [SerializeField]
        private SharedTransform target1;
        [SerializeField]
        private SharedTransform target2;
        [SerializeField]
        private SharedTransform target3;
        [SerializeField]
        private SharedTransform storeResult;

        public override TaskStatus OnUpdate()
        {
            if (target1.Value)
            {
                storeResult.Value = target1.Value;
            }
            else if(target2.Value)
            {
                storeResult.Value = target2.Value;
            }
            else
            {
                storeResult.Value = target3.Value;   
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target1 = null;
            target2 = null;
            target3 = null;
            storeResult = null;
        }
    }
}