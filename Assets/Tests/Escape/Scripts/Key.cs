using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    public class Key : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            Global.GetService<EventManager>().Send((int) EventId.KeyCount);
        }
    }
}