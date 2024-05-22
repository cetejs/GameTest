using BehaviorDesigner;
using GameFramework;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class SendAlarmEvent : Action
    {
        [SerializeField]
        private EventId eventId;
        [SerializeField]
        private SharedFloat radius;
        [SerializeField]
        private SharedTransform warner;
        
        public override TaskStatus OnUpdate()
        {
            GameData<float, Transform> data = ReferencePool.Instance.Get<GameData<float, Transform>>();
            data.Item1 = radius.Value;
            data.Item2 = warner.Value;
            EventManager.Instance.Send((int)eventId, data);
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