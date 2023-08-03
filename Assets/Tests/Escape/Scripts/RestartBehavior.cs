using BehaviorDesigner;
using GameFramework;
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
            EventManager.Instance.Register((int) EventId.Reborn, Reborn);
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister((int) EventId.Reborn, Reborn);
        }

        private void Reborn(EventBody body)
        {
            DelayedActionManager.Instance.AddAction(behavior.Restart, 0.5f);
        }
    }
}