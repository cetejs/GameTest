using GameFramework;
using UnityEngine;

namespace Escape
{
    public class Key : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            EventManager.Instance.Send((int) EventId.KeyCount);
        }
    }
}