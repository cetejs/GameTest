using UnityEngine;

namespace Escape
{
    [CreateAssetMenu(menuName = "Config/PlayerEnergy")]
    public class PlayerEnergy : ScriptableObject
    {
        public float curEnergy = 100;
        public float maxEnergy = 100;
        public float consumeSpeed = 10f;
        public float resumeSpeed = 10f;

        public void ResumeToMax()
        {
            curEnergy = maxEnergy;
        }

        public bool Consume()
        {
            if (curEnergy > 0)
            {
                curEnergy -= consumeSpeed * Time.deltaTime;
                return true;
            }

            return false;
        }

        public void Resume()
        {
            curEnergy = Mathf.Clamp(curEnergy + resumeSpeed * Time.deltaTime, 0, maxEnergy);
        }
    }
}