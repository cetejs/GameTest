using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;

namespace Escape
{
    public class KeyCounter : MonoBehaviour
    {
        private void Start()
        {
            Data<int> data = ReferencePool.Get<Data<int>>();
            data.item = transform.childCount;
            Global.GetService<EventManager>().Send((int) EventId.KeyNumber, data);
        }
    }
}