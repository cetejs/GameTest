using GameFramework.Generic;
using GameFramework.InputService;
using UnityEngine;

namespace GameFramework
{
    public class FreeLookCamera : FollowTargetCamera
    {
        [SerializeField]
        private float moveSpeed = 5.0f;
        [SerializeField]
        private float turnSpeed = 3.0f;
        [SerializeField]
        private float turnSmoothing = 15.0f;
        [SerializeField]
        private float minTiltAngle = -45.0f;
        [SerializeField]
        private float maxTiltAngle = 75.0f;
        [SerializeField]
        private float joystickAxisSpeed = 0.1f;
        [SerializeField]
        private bool isLockScreen;

        private InputManager input;
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
        }

        private void Start()
        {
            input = Global.GetService<InputManager>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                isLockScreen = !isLockScreen;
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
            if (!isLockScreen && !input.IsPointerOverGameObject())
            {
                float axisSpeed = input.InputDevice == InputDevice.MouseKeyboard ? 1.0f : joystickAxisSpeed;
                float x = input.GetAxis("Mouse X") * axisSpeed;
                float y = input.GetAxis("Mouse Y") * axisSpeed;
                lookAngle += x * turnSpeed;
                tiltAngle -= y * turnSpeed;
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