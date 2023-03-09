using BehaviorDesigner;
using GameFramework.DelayedActionService;
using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    public class RestartBehavior : MonoBehaviour
    {
        [SerializeField]
        private Behavior behavior;

        private void Start()
        {
            behavior = GetComponent<Behavior>();
            Global.GetService<EventManager>().Register((int) EventId.Reborn, Reborn);
        }

        private void OnDestroy()
        {
            if (Global.IsApplicationQuitting)
            {
                return;
            }

            Global.GetService<EventManager>().Unregister((int) EventId.Reborn, Reborn);
        }

        private void Reborn(EventBody body)
        {
            Global.GetService<DelayedActionManager>().AddAction(behavior.Restart, 0.5f);
        }
    }
}