using BehaviorDesigner;
using UnityEngine;

namespace Escape
{
    [TaskGroup("Escape")]
    public class BindIndicator : Action
    {
        [SerializeField]
        private SharedFloat fieldOfView;
        [SerializeField]
        private SharedFloat viewDistance;
        [SerializeField]
        private SharedVector3 viewOffset;
        [SerializeField]
        private SharedFloat hearDistance;
        [SerializeField]
        private MeshRenderer seeIndicator;
        [SerializeField]
        private MeshRenderer hearIndicator;

        private float lastSeeFOV;
        private float lastSeeDis;
        private Vector3 lastSeeOffset;
        private float lastHearDis;

        public override TaskStatus OnUpdate()
        {
            if (lastSeeFOV != fieldOfView.Value)
            {
                lastSeeFOV = fieldOfView.Value;
                seeIndicator.material.SetFloat("_Angle", fieldOfView.Value);
            }

            if (lastSeeDis != viewDistance.Value)
            {
                lastSeeDis = viewDistance.Value;
                float radius = viewDistance.Value / 5f;
                seeIndicator.transform.localScale = new Vector3(radius, 1, radius);
            }

            if (lastSeeOffset != viewOffset.Value)
            {
                lastSeeOffset = viewOffset.Value;
                seeIndicator.transform.localPosition = viewOffset.Value;
            }
            
            if (lastHearDis != hearDistance.Value && hearIndicator)
            {
                lastHearDis = hearDistance.Value;
                float radius = hearDistance.Value / 5f;
                hearIndicator.transform.localScale = new Vector3(radius, 1, radius);
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            fieldOfView = 0f;
            viewDistance = 0f;
            viewOffset = Vector3.zero;
            hearDistance = 0f;
            seeIndicator = null;
            hearIndicator = null;
        }
    }
}
