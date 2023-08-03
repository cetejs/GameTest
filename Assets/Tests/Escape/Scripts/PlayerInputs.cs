using GameFramework;
using UnityEngine;

namespace Escape
{
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField]
        protected InputControl input;
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

        public bool IsCurrentDeviceMouse
        {
            get { return input.InputDevice == InputDevice.MouseKeyboard; }
        }

        protected virtual void Update()
        {
            look.x = input.GetAxis("LookX");
            look.y = input.GetAxis("LookY");
            zoom = input.GetAxis("Zoom");
            move.x = input.GetAxis("MoveX");
            move.y = input.GetAxis("MoveY");
            slow = input.GetButton("Slow");
            sprint = input.GetButton("Sprint");
        }
    }
}