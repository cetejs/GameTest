using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class StoreSeekTarget : Action
    {
        [SerializeField]
        private SharedTransform seeObj;
        [SerializeField]
        private SharedTransform hearObj;
        [SerializeField]
        private SharedTransform storeResult;

        public override TaskStatus OnUpdate()
        {
            if (seeObj.Value)
            {
                storeResult.Value = seeObj.Value;
            }
            else
            {
                storeResult.Value = hearObj.Value;
            }

            return TaskStatus.Success;
        }
    }
}