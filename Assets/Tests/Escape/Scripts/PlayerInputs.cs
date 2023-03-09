using UnityEngine;
using UnityEngine.InputSystem;

namespace Escape
{
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput input;
        [SerializeField]
        private Vector2 look;
        [SerializeField]
        private Vector2 move;
        [SerializeField]
        private float zoom;
        [SerializeField]
        private bool slow;
        [SerializeField]
        private bool sprint;
        [SerializeField]
        private bool operate;

        public Vector2 Look
        {
            get { return look; }
        }

        public float Zoom
        {
            get { return zoom; }
        }

        public Vector2 Move
        {
            get { return move; }
        }

        public bool IsSlow
        {
            get { return slow; }
            set { slow = value; }
        }

        public bool IsSprint
        {
            get { return sprint; }
            set { sprint = value; }
        }

        public bool IsOperate
        {
            get { return operate; }
            set { operate = value; }
        }

        public bool IsCurrentDeviceMouse
        {
            get { return input.currentControlScheme == "KeyboardMouse"; }
        }

        public void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        public void OnZoom(InputValue value)
        {
            zoom = value.Get<float>();
        }

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }

        public void OnSlow(InputValue value)
        {
            slow = value.isPressed;
        }

        public void OnOperate(InputValue value)
        {
            operate = value.isPressed;
        }

        private void OnEnable()
        {
            input.enabled = true;
        }

        private void OnDisable()
        {
            input.enabled = false;
        }
    }
}