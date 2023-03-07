using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner;
using UnityEngine;
using UnityEngine.AI;

namespace Escape
{
    [TaskCategory("Escape")]
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
            return TaskStatus.Success;
        }
    }
}