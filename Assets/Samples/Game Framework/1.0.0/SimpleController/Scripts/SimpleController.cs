using GameFramework.Generic;
using GameFramework.InputService;
using UnityEngine;

namespace GameFramework
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleController : MonoBehaviour
    {
        [Header("----- Generic -----")]
        [SerializeField]
        private float moveSpeed = 1.0f;
        [SerializeField]
        private float turnSpeed = 30.0f;
        [SerializeField]
        private float jumpSpeed = 3.0f;
        [SerializeField]
        private float jumpTime = 0.3f;
        [SerializeField]
        private float gravity = 9.81f;
        [SerializeField]
        private float springMulti = 1.5f;
        private InputManager input;
        private CharacterController cc;
        private Transform cam;
        private Vector3 camForward;
        private Vector3 moveInput;
        private float jumpCounter;

        [Header("-----  Input -----")]
        [SerializeField]
        private bool isJumpDown;
        [SerializeField]
        private bool isSpringing;
        [SerializeField]
        private bool isStrafing;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
        }

        private void Start()
        {
            cam = Camera.main.transform;
            input = Global.GetService<InputManager>();
        }

        private void OnDisable()
        {
            isJumpDown = false;
            isSpringing = false;
        }

        private void Update()
        {
            UpdateInput();
            UpdateMovement();
            UpdateRotation();
        }

        private void UpdateInput()
        {
            float h = input.GetAxis("Horizontal");
            float v = input.GetAxis("Vertical");

            if (cam != null)
            {
                camForward = cam.forward;
                camForward.y = 0.0f;
                Vector3 forward = camForward.normalized;
                moveInput = forward * v + cam.right * h;
            }
            else
            {
                moveInput = Vector3.forward * v + Vector3.right * h;
            }

            if (input.GetButtonDown("Button0"))
            {
                isJumpDown = true;
            }

            if (input.GetButtonDown("Button1"))
            {
                isSpringing = true;
            }
            else if (input.GetButtonUp("Button1"))
            {
                isSpringing = false;
            }

            if (input.GetButtonDown("LeftStickClick"))
            {
                isSpringing = !isSpringing;
            }

            if (input.GetButtonDown("RightStickClick"))
            {
                isStrafing = !isStrafing;
            }
        }

        private void UpdateMovement()
        {
            if (isJumpDown && cc.isGrounded)
            {
                jumpCounter = jumpTime;
            }

            isJumpDown = false;

            float verticalSpeed = 0.0f;
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

            float forwardSpeed = isSpringing ? moveSpeed * springMulti : moveSpeed;

            if (moveInput.sqrMagnitude > 1)
            {
                moveInput.Normalize();
            }

            cc.Move((moveInput * forwardSpeed + Vector3.up * verticalSpeed) * Time.deltaTime);
        }

        private void UpdateRotation()
        {
            Vector3 lookForward;
            if (isStrafing)
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