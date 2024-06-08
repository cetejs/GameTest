using UnityEngine;

namespace GameFramework.Samples.SimpleController
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 1f;
        [SerializeField]
        private float turnSpeed = 30f;
        [SerializeField]
        private float jumpSpeed = 3f;
        [SerializeField]
        private float jumpTime = 0.3f;
        [SerializeField]
        private float gravity = 9.81f;
        [SerializeField]
        private float springMultiplier = 1.5f;
        private SimpleInputs input;
        private CharacterController cc;
        private Transform cam;
        private Vector3 camForward;
        private Vector3 moveInput;
        private float jumpCounter;
        private bool isJumpDown;

        private void Awake()
        {
            input = GetComponentInParent<SimpleInputs>();
            cc = GetComponent<CharacterController>();
        }

        private void Start()
        {
            cam = Camera.main.transform;
        }

        private void Update()
        {
            UpdateInput();
            UpdateMovement();
            UpdateRotation();
        }

        private void UpdateInput()
        {
            if (cam != null)
            {
                camForward = cam.forward;
                camForward.y = 0f;
                Vector3 forward = camForward.normalized;
                moveInput = forward * input.Move.y + cam.right * input.Move.x;
            }
            else
            {
                moveInput = Vector3.forward * input.Move.y + Vector3.right * input.Move.x;
            }
        }

        private void UpdateMovement()
        {
            if (input.IsJump && cc.isGrounded)
            {
                jumpCounter = jumpTime;
            }

            input.IsJump = false;
            float verticalSpeed = 0f;
            if (jumpCounter > 0)
            {
                jumpCounter -= Time.deltaTime;
                verticalSpeed = jumpSpeed;
            }
            else if (cc.isGrounded)
            {
                verticalSpeed = -gravity;
            }
            else
            {
                verticalSpeed -= gravity;
            }

            float forwardSpeed = input.IsSprint ? moveSpeed * springMultiplier : moveSpeed;

            if (moveInput.sqrMagnitude > 1)
            {
                moveInput.Normalize();
            }

            cc.Move((moveInput * forwardSpeed + Vector3.up * verticalSpeed) * Time.deltaTime);
        }

        private void UpdateRotation()
        {
            Vector3 lookForward;
            if (input.IsStrafe)
            {
                lookForward = camForward;
            }
            else
            {
                lookForward = moveInput;
            }

            if (lookForward.sqrMagnitude > 0)
            {
                Vector3 targetEuler = transform.eulerAngles;
                Quaternion freeRot = Quaternion.LookRotation(lookForward, transform.up);
                targetEuler.y = freeRot.eulerAngles.y;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetEuler), turnSpeed * Time.deltaTime);
            }
        }
    }
}