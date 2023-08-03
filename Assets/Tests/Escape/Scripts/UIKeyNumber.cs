using GameFramework;
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
            EventManager.Instance.Register((int) EventId.KeyNumber, body =>
            {
                num = remain = body.GetData<GameData<int>>().Item;
                text.text = string.Concat(num, "/", num);
                body.Unregister();
            });

            EventManager.Instance.Register((int) EventId.KeyCount, body =>
            {
                text.text = string.Concat(--remain, "/", num);
                if (remain == 0)
                {
                    EventManager.Instance.Send((int) EventId.OpenDoor);
                    body.Unregister();
                }
            });
        }
    }
}