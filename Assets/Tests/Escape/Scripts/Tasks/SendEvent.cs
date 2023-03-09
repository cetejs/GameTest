using BehaviorDesigner;
using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class SendEvent : Action
    {
        [SerializeField]
        private EventId eventId;
        
        public override TaskStatus OnUpdate()
        {
            Global.GetService<EventManager>().Send((int)eventId);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            eventId = 0;
        }
    }
}