using GameFramework;
using UnityEngine;

namespace Escape
{
    public class KeyCounter : MonoBehaviour
    {
        private void Start()
        {
            GameData<int> data = ReferencePool.Instance.Get<GameData<int>>();
            data.Item = transform.childCount;
            EventManager.Instance.Send((int) EventId.KeyNumber, data);
        }
    }
}