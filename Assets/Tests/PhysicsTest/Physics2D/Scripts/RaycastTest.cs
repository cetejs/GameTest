using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsTest
{
    public class RaycastTest : MonoBehaviour
    {
        public Line line;
        private Collider2D[] cols;
        private List<HitInfo2D> hits = new List<HitInfo2D>();
        private HitInfo2D hit;
        private bool hitted;
        
        private void Awake()
        {
            cols = FindObjectsOfType<Collider2D>();
        }

        private void Update()
        {
            Vector3 p1 = line.p1.position;
            Vector3 p2 = line.p2.position;
            Vector3 vec = p2 - p1;
            hits.Clear();
            for (int i = 0; i < cols.Length; i++)
            {
                if (Physics2DUtils.Raycast(p1, vec.normalized, vec.magnitude, out hit, cols[i]))
                {
                    hits.Add(hit);
                }
            }

            hitted = false;
            float min = Single.MaxValue;
            for (int i = 0; i < hits.Count; i++)
            {
                float dis = Vector3.SqrMagnitude(hits[i].point - p1);
                if (dis < min)
                {
                    min = dis;
                    hit = hits[i];
                    hitted = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (hitted)
            {
                Color color = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit.point, 0.2f);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hit.point, hit.point + hit.normal);
                Gizmos.color = color;
            }
        }
    }
}