using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class StoreSeekTarget : Action
    {
        [SerializeField] [RequiredField]
        private SharedTransform seeObj;
        [SerializeField] [RequiredField]
        private SharedTransform hearObj;
        [SerializeField] [RequiredField]
        private SharedTransform alarmObj;
        [SerializeField] [RequiredField]
        private SharedTransform storeResult;

        public override TaskStatus OnUpdate()
        {
            if (seeObj.Value)
            {
                storeResult.Value = seeObj.Value;
            }
            else if(hearObj.Value)
            {
                storeResult.Value = hearObj.Value;
            }
            else
            {
                storeResult.Value = alarmObj.Value;   
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            seeObj = null;
            hearObj = null;
            alarmObj = null;
            storeResult = null;
        }
    }
}