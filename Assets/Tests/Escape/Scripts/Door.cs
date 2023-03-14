using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    public class Door : MonoBehaviour
    {
        private void Awake()
        {
            Global.GetService<EventManager>().Register((int)EventId.OpenDoor, body =>
            {
                gameObject.SetActive(false);
                body.Unregister();
            });
        }
    }
}

