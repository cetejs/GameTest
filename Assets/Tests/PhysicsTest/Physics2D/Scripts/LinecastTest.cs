using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsTest
{
    public class LinecastTest : MonoBehaviour
    {
        public Line line;
        private Line[] lines;
        private Collider2D[] cols;
        private HitInfo2D[] hitInfos = new HitInfo2D[2];
        private List<HitInfo2D> hits = new List<HitInfo2D>();

        private void Awake()
        {
            lines = FindObjectsOfType<Line>();
            cols = FindObjectsOfType<Collider2D>();
        }

        private void Update()
        {
            Vector3 p1 = line.p1.position;
            Vector3 p2 = line.p2.position;
            hits.Clear();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == line)
                {
                    continue;
                }
                
                if (Physics2DUtils.Linecast(p1, p2, lines[i].p1.position, lines[i].p2.position, out Vector3 hit))
                {
                    hits.Add(new HitInfo2D()
                    {
                        point = hit
                    });
                }
            }

            for (int i = 0; i < cols.Length; i++)
            {
                int len = Physics2DUtils.Linecast(p1, p2, hitInfos, cols[i]);
                for (int j = 0; j < len; j++)
                {
                    hits.Add(hitInfos[j]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < hits.Count; i++)
            {
                HitInfo2D hit = hits[i];
                Color color = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit.point, 0.1f);
                if (hit.normal.sqrMagnitude > 0)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(hit.point, hit.point + hit.normal);
                }

                Gizmos.color = color;
            }
        }
    }
}