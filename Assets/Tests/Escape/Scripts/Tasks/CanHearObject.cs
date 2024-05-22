using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class CanHearObject : IntervalConditional
    {
        [SerializeField]
        private SharedTag listeningTag = Tag.Untagged;
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

        private Transform target;
        private CharacterController cc;

        public override void OnStart()
        {
            base.OnStart();
            if (!target)
            {
                target = GameObject.FindGameObjectWithTag(listeningTag.Value).transform;
                cc = target.GetComponent<CharacterController>();
            }
        }

        public override TaskStatus OnConditionalUpdate()
        {
            storeResult.Value = null;
            if (Vector3.SqrMagnitude(transform.position - target.position) > hearRadius.Value * hearRadius.Value)
            {
                return TaskStatus.Failure;
            }

            if (cc.velocity.sqrMagnitude < hearSpeed.Value * hearSpeed.Value)
            {
                return TaskStatus.Failure;
            }

            storeResult.Value = target;
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

        public override void OnReset()
        {
            base.OnReset();
            listeningTag = Tag.Untagged;
            hearRadius = 10f;
            hearSpeed = 3f;
            storeResult = null;
            drawGizmos = true;
            viewColor = new Color(1f, 0.92f, 0.016f, 0.1f);
        }
    }
}