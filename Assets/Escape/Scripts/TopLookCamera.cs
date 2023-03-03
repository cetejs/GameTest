using GameFramework;
using UnityEngine;

namespace Escape
{
    public class TopLookCamera : FollowTargetCamera
    {
        [SerializeField]
        private float followSpeed = 3f;
        [SerializeField]
        private float moveSpeed = 30f;
        [SerializeField]
        private Vector2 minMoveRange = Vector3.zero;
        [SerializeField]
        private Vector2 maxMoveRange = Vector3.one * 25f;

        private PlayerInputs input;
        private bool isLooking;

        protected override void Awake()
        {
            base.Awake();
            input = GetComponentInParent<PlayerInputs>();
        }

        protected override void Update()
        {
            if (input.Look.sqrMagnitude > 0.01f)
            {
                isLooking = true;
                Vector2 look = input.Look.normalized;
                Vector3 pos = transform.position;
                Vector3 lookPos = pos + new Vector3(look.x, 0f, look.y);
                Vector3 targetPos = target.transform.position;
                pos = Vector3.Lerp(pos, lookPos, moveSpeed * Time.deltaTime);
                pos.x = Mathf.Clamp(pos.x, targetPos.x - maxMoveRange.x, targetPos.x + maxMoveRange.x);
                pos.z = Mathf.Clamp(pos.z, targetPos.z - maxMoveRange.y, targetPos.z + maxMoveRange.y);
                transform.position = pos;
            }
            else
            {
                isLooking = false;
            }
        }

        protected override void FollowTarget()
        {
            if (isLooking)
            {
                return;
            }

            Vector3 pos = transform.position;
            Vector3 targetPos = target.position;
            if (Mathf.Abs(pos.x - targetPos.x) > minMoveRange.x ||
                Mathf.Abs(pos.z - targetPos.z) > minMoveRange.y)
            {
                targetPos.y = pos.y;
                transform.position = Vector3.Lerp(pos, targetPos, followSpeed * Time.deltaTime);
            }
        }
    }
}