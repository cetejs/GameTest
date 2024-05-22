using BehaviorDesigner;
using UnityEngine;
using UnityEngine.AI;

namespace Escape
{
    [TaskGroup("Escape")]
    public class StopNavPath : Action
    {
        [SerializeField]
        private NavMeshAgent agent;

        public override TaskStatus OnUpdate()
        {
            if (!agent)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            agent.isStopped = true;
            agent.updateRotation = false;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            agent = null;
        }
    }
}