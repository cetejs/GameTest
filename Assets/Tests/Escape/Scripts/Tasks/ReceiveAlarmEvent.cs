using BehaviorDesigner;
using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class ReceiveAlarmEvent : Action
    {
        [SerializeField]
        private EventId eventId;
        [SerializeField] [RequiredField]
        private SharedFloat radius;
        [SerializeField] [RequiredField]
        private SharedTransform warner;

        public override void OnStart()
        {
            base.OnStart();
            Global.GetService<EventManager>().Register((int)eventId, EventHandler);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            Global.GetService<EventManager>().Unregister((int)eventId, EventHandler);
        }

        private void EventHandler(EventBody body)
        {
            Data<float, Transform> data = body.GetData<Data<float, Transform>>();
            radius.Value = data.item1;
            warner.Value = data.item2;
        }
        
        public override void OnReset()
        {
            eventId = 0;
            radius = 0f;
            warner = null;
        }
    }
}