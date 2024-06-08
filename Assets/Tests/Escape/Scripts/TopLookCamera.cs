using GameFramework;
using GameFramework.Samples.SimpleController;
using UnityEngine;

namespace Escape
{
    public class TopLookCamera : FollowTargetCamera
    {
        [InspectorGroup("Look")]
        [SerializeField]
        private float followSpeed = 3f;
        [SerializeField]
        private float moveSpeed = 30f;
        [SerializeField]
        private Vector2 minMoveRange = Vector3.zero;
        [SerializeField]
        private Vector2 maxMoveRange = Vector3.one * 25f;
        [InspectorGroup("Zoom")]
        [SerializeField]
        private float zoomSpeed = 30f;
        [SerializeField]
        private Vector2 zoomRange = new Vector2(20, 60);

        private PlayerInputs input;

        protected override void Awake()
        {
            base.Awake();
            input = GetComponentInParent<PlayerInputs>();
        }

        protected override void FollowTarget()
        {
            Zoom();
            if (Look())
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

        private bool Look()
        {
            if (input.Look.sqrMagnitude > 0.01f)
            {
                Vector2 look = input.Look.normalized;
                Vector3 pos = transform.position;
                Vector3 lookPos = pos + new Vector3(look.x, 0f, look.y);
                Vector3 targetPos = target.transform.position;
                pos = Vector3.Lerp(pos, lookPos, moveSpeed * Time.deltaTime);
                pos.x = Mathf.Clamp(pos.x, targetPos.x - maxMoveRange.x, targetPos.x + maxMoveRange.x);
                pos.z = Mathf.Clamp(pos.z, targetPos.z - maxMoveRange.y, targetPos.z + maxMoveRange.y);
                transform.position = pos;
                return true;
            }

            return false;
        }

        private void Zoom()
        {
            float zoom = input.Zoom;
            zoom *= zoomSpeed * Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(pos.y - zoom, zoomRange.x, zoomRange.y);
            transform.position = pos;
        }
    }
}