using System;
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

        protected virtual void Update()
        {
            look.x = input.GetAxisRaw("LookX");
            look.y = input.GetAxisRaw("LookY");
            move.x = input.GetAxisRaw("MoveX");
            move.y = input.GetAxisRaw("MoveY");
            zoom = input.GetAxis("Zoom");
            slow = input.GetButton("Slow");
            sprint = input.GetButton("Sprint");
        }

        private void OnDisable()
        {
            look = Vector2.zero;
            move = Vector2.zero;
            zoom = 0f;
            slow = false;
            sprint = false;
        }
    }
}