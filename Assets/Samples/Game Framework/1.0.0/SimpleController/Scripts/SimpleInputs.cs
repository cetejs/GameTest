using UnityEngine;
using UnityEngine.InputSystem;

namespace GameFramework
{
    public class SimpleInputs : MonoBehaviour, IFreeLookInput
    {
        [SerializeField]
        private PlayerInput input;
        [SerializeField]
        private Vector2 look;
        [SerializeField]
        private Vector2 move;
        [SerializeField]
        private bool jump;
        [SerializeField]
        private bool sprint;
        [SerializeField]
        private bool strafe;

        public Vector2 Look
        {
            get { return look; }
        }

        public Vector2 Move
        {
            get { return move; }
        }

        public bool IsJump
        {
            get { return jump; }
            set { jump = value; }
        }

        public bool IsSprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        public bool IsStrafe
        {
            get { return strafe; }
            set { strafe = value; }
        }

        public bool IsCurrentDeviceMouse
        {
            get { return input.currentControlScheme == "KeyboardMouse"; }
        }

        public void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            jump = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }

        public void OnStrafe(InputValue value)
        {
            strafe = !strafe;
        }
    }
}