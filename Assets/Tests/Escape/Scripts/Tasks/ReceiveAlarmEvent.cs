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
        private SharedInt eventId;
        [SerializeField] [RequiredField]
        private SharedFloat radius;
        [SerializeField] [RequiredField]
        private SharedTransform warner;

        public override void OnStart()
        {
            base.OnStart();
            Global.GetService<EventManager>().Register(eventId.Value, EventHandler);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            Global.GetService<EventManager>().Unregister(eventId.Value, EventHandler);
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