using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework
{
    public class FreeLookCamera : FollowTargetCamera
    {
        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private float turnSpeed = 3f;
        [SerializeField]
        private float turnSmoothing = 15f;
        [SerializeField]
        private float minTiltAngle = -45f;
        [SerializeField]
        private float maxTiltAngle = 75f;
        [SerializeField]
        private bool lookScreen;

        private IFreeLookInput lookInput;
        private Transform pivot;
        private float lookAngle;
        private float tiltAngle;
        private Vector3 pivotEuler;
        private Quaternion lookTargetRot;
        private Quaternion tiltTargetRot;

        protected override void Awake()
        {
            base.Awake();
            pivot = cam.transform.parent;
            pivotEuler = pivot.eulerAngles;
            lookTargetRot = transform.localRotation;
            tiltTargetRot = pivot.localRotation;
            lookInput = GetComponentInParent<IFreeLookInput>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                lookScreen = !lookScreen;
            }

            HandleRotation();
        }

        protected override void FollowTarget()
        {
            if (moveSpeed > 0)
            {
                transform.position = Vector3.Lerp(transform.position, Target.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Target.position;
            }
        }

        public void ResetRotation()
        {
            lookAngle = target.eulerAngles.y;
        }

        private void HandleRotation()
        {
            if (!lookScreen)
            {
                lookAngle += lookInput.Look.x * turnSpeed;
                tiltAngle -= lookInput.Look.y * turnSpeed;
            }
            
            tiltAngle = Mathf.Clamp(tiltAngle, minTiltAngle, maxTiltAngle);
            lookTargetRot = Quaternion.Euler(0, lookAngle, 0);
            tiltTargetRot = Quaternion.Euler(tiltAngle, pivotEuler.y, pivotEuler.z);

            if (turnSmoothing > 0)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, lookTargetRot, turnSmoothing * Time.deltaTime);
                pivot.localRotation = Quaternion.Slerp(pivot.localRotation, tiltTargetRot, turnSmoothing * Time.deltaTime);
            }
            else
            {
                transform.localRotation = lookTargetRot;
                pivot.localRotation = tiltTargetRot;
            }
        }
    }
}