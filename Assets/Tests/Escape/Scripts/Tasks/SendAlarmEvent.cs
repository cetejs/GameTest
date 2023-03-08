using BehaviorDesigner;
using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class SendAlarmEvent : Action
    {
        [SerializeField]
        private SharedInt eventId;
        [SerializeField]
        private SharedFloat radius;
        [SerializeField]
        private SharedTransform warner;
        
        public override TaskStatus OnUpdate()
        {
            Data<float, Transform> data = ReferencePool.Get<Data<float, Transform>>();
            data.item1 = radius.Value;
            data.item2 = warner.Value;
            Global.GetService<EventManager>().Send(eventId.Value, data);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            eventId = 0;
            radius = 0f;
            warner = null;
        }
    }
}