using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskCategory("Escape")]
    public class CanHearObject : IntervalConditional
    {
        [SerializeField]
        private SharedTransform listeningTarget;
        [SerializeField]
        private SharedFloat hearRadius = 10f;
        [SerializeField]
        private SharedFloat hearSpeed = 3f;
        [SerializeField]
        private SharedTransform storeResult;
        [SerializeField]
        private bool drawGizmos = true;
        [SerializeField]
        private Color viewColor = new Color(1f, 0.92f, 0.016f, 0.1f);

        private CharacterController cc;

        public override void OnAwake()
        {
           cc = listeningTarget.Value.GetComponent<CharacterController>();
        }

        public override TaskStatus OnConditionalUpdate()
        {
            storeResult.Value = null;
            if (Vector3.SqrMagnitude(transform.position - listeningTarget.Value.position) > hearRadius.Value * hearRadius.Value)
            {
                return TaskStatus.Failure;
            }

            if (cc.velocity.sqrMagnitude < hearSpeed.Value * hearSpeed.Value)
            {
                return TaskStatus.Failure;
            }

            storeResult.Value = listeningTarget.Value;
            return TaskStatus.Success;
        }
        
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!drawGizmos)
            {
                return;
            }
            
            Color oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = viewColor;
            UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, hearRadius.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}