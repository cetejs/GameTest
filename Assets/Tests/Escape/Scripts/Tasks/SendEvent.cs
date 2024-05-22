using BehaviorDesigner;
using GameFramework;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class SendEvent : Action
    {
        [SerializeField]
        private EventId eventId;
        
        public override TaskStatus OnUpdate()
        {
            EventManager.Instance.Send((int)eventId);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            eventId = 0;
        }
    }
}