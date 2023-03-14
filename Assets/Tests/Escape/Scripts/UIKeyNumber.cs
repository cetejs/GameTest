using GameFramework.EventPoolService;
using GameFramework.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Escape
{
    public class UIKeyNumber : MonoBehaviour
    {
        [SerializeField]
        private Text text;
        private int num;
        private int remain;

        private void Awake()
        {
            Global.GetService<EventManager>().Register((int) EventId.KeyNumber, body =>
            {
                num = remain = body.GetData<Data<int>>().item;
                text.text = string.Concat(num, "/", num);
                body.Unregister();
            });

            Global.GetService<EventManager>().Register((int) EventId.KeyCount, body =>
            {
                text.text = string.Concat(--remain, "/", num);
                if (remain == 0)
                {
                    Global.GetService<EventManager>().Send((int) EventId.OpenDoor);
                    body.Unregister();
                }
            });
        }
    }
}