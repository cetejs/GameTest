using BehaviorDesigner;
using GameFramework;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class ReceiveAlarmEvent : Action
    {
        [SerializeField]
        private EventId eventId;
        [SerializeField]
        private SharedFloat radius;
        [SerializeField]
        private SharedTransform warner;

        public override void OnStart()
        {
            base.OnStart();
            EventManager.Instance.Register((int)eventId, EventHandler);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            EventManager.Instance.Unregister((int)eventId, EventHandler);
        }

        private void EventHandler(EventBody body)
        {
            GameData<float, Transform> data = body.GetData<GameData<float, Transform>>();
            radius.Value = data.Item1;
            warner.Value = data.Item2;
        }
        
        public override void OnReset()
        {
            eventId = 0;
            radius = 0f;
            warner = null;
        }
    }
}