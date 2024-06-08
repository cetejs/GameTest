using UnityEngine;

namespace GameFramework.Samples.SimpleController
{
    public class ProtectCameraFromWallClip : MonoBehaviour
    {
        [SerializeField]
        private float clipMoveTime = 0.05f;
        [SerializeField]
        private float returnTime = 0.5f;
        [SerializeField]
        private float closestDistance = 0.1f;
        [SerializeField]
        private LayerMask checkLayerMask = 1 << 0;

        private Camera cam;
        private Transform pivot;
        private float originalDist;
        private float currentDist;
        private float moveVelocity;
        private readonly RaycastHit[] hitInfos = new RaycastHit[10];

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();
            pivot = cam.transform.parent;

            originalDist = -cam.transform.localPosition.z;
            currentDist = originalDist;
        }

        private void LateUpdate()
        {
            float targetDist = originalDist;
            float nearest = Mathf.Infinity;
            bool hasSmoothing = false;
            int length = Physics.RaycastNonAlloc(pivot.position, -pivot.forward, hitInfos, originalDist, checkLayerMask);
            for (int i = 0; i < length; i++)
            {
                RaycastHit hit = hitInfos[i];

                if (hit.collider.isTrigger)
                {
                    continue;
                }

                if (hit.distance < nearest)
                {
                    nearest = hit.distance;
                    targetDist = hit.distance;
                    hasSmoothing = true;
                }
            }

#if UNITY_EDITOR
            if (hasSmoothing)
            {
                Debug.DrawRay(pivot.position, -pivot.forward * targetDist, Color.red);
            }
#endif

            targetDist = Mathf.Clamp(targetDist, closestDistance, originalDist);
            float smoothTime = currentDist > targetDist ? clipMoveTime : returnTime;
            currentDist = Mathf.SmoothDamp(currentDist, targetDist, ref moveVelocity, smoothTime);
            cam.transform.localPosition = -Vector3.forward * currentDist;
        }
    }
}