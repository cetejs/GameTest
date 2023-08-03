using GameFramework;
using UnityEngine;

namespace Escape
{
    public class Door : MonoBehaviour
    {
        private void Awake()
        {
            EventManager.Instance.Register((int)EventId.OpenDoor, body =>
            {
                gameObject.SetActive(false);
                body.Unregister();
            });
        }
    }
}

