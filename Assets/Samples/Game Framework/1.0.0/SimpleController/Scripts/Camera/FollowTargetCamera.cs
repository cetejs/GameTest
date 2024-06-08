using UnityEngine;

namespace GameFramework.Samples.SimpleController
{
    public abstract class FollowTargetCamera : GameBehaviour
    {
        [SerializeField]
        protected Transform target;
        [SerializeField]
        protected UpdateType updateType = UpdateType.LateUpdate;

        protected Camera cam;

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        public void SetFollowTarget(Transform target)
        {
            this.target = target;
        }

        public void ManualUpdate()
        {
            if (target == null)
            {
                return;
            }

            if (updateType != UpdateType.ManualUpdate)
            {
                return;
            }

            FollowTarget();
        }

        protected virtual void Awake()
        {
            cam = GetComponentInChildren<Camera>();
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            if (updateType != UpdateType.FixedUpdate)
            {
                return;
            }

            FollowTarget();
        }

        protected virtual void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            if (updateType != UpdateType.LateUpdate)
            {
                return;
            }

            FollowTarget();
        }

        protected abstract void FollowTarget();

        protected enum UpdateType
        {
            FixedUpdate,
            LateUpdate,
            ManualUpdate
        }
    }
}