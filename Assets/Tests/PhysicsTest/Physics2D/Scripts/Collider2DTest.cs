using UnityEngine;

namespace PhysicsTest
{
    public class Collider2DTest : MonoBehaviour
    {
        private Collider2D[] cs;

        private void OnEnable()
        {
            cs = FindObjectsOfType<Collider2D>();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < cs.Length; i++)
            {
                cs[i].IsTrigger = false;
            }
            
            for (int i = 0; i < cs.Length; i++)
            {
                for (int j = i + 1; j < cs.Length; j++)
                {
                    if (cs[i].gameObject == cs[j].gameObject)
                    {
                        continue;
                    }

                    if (Physics2DUtils.IsIntersect(cs[i], cs[j]))
                    {
                        cs[i].IsTrigger = cs[j].IsTrigger = true;
                        cs[i].HitInfo = new HitInfo2D()
                        {
                            other = cs[j]
                        };

                        cs[j].HitInfo = new HitInfo2D()
                        {
                            other = cs[i]
                        };
                    }
                }
            }
        }
    }
}