using UnityEngine;
using UnityEngine.UI;

namespace Escape
{
    public class UIPlayerEnergy : MonoBehaviour
    {
        [SerializeField]
        private Scrollbar bar;
        [SerializeField]
        private PlayerEnergy energy;

        private void Update()
        {
            bar.size = energy.curEnergy / energy.maxEnergy;
        }
    }
}