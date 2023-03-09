using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    public class ResetBirthPoint : MonoBehaviour
    {
        [SerializeField]
        private Vector3 birthPosition;
        [SerializeField]
        private Quaternion birthRotation;

        private void Start()
        {
            birthPosition = transform.position;
            birthRotation = transform.rotation;
            Global.GetService<EventManager>().Register((int) EventId.Reborn, Reborn);
        }

        private void OnDestroy()
        {
            if (Global.IsApplicationQuitting)
            {
                return;
            }

            Global.GetService<EventManager>().Unregister((int) EventId.Reborn, Reborn);
        }

        private void Reborn(EventBody body)
        {
            transform.position = birthPosition;
            transform.rotation = birthRotation;
        }
    }
}