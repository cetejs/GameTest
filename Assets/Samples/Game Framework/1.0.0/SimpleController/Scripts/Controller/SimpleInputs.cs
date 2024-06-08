using UnityEngine;

namespace GameFramework.Samples.SimpleController
{
    public class SimpleInputs : MonoBehaviour, IFreeLookInput
    {
        [SerializeField]
        protected InputControl input;
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

        public InputDevice InputDevice
        {
            get { return input.InputDevice; }
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

        protected virtual void Update()
        {
            look.x = input.GetAxis("LookX");
            look.y = input.GetAxis("LookY");
            move.x = input.GetAxisRaw("MoveX");
            move.y = input.GetAxisRaw("MoveY");
            jump = input.GetButton("Jump");
            sprint = input.GetButton("Sprint");
            if (input.GetButtonDown("Strafe"))
            {
                strafe = !strafe;
            }
        }

        protected void OnDisable()
        {
            look = Vector2.zero;
            move = Vector2.zero;
            jump = false;
            sprint = false;
        }
    }
}