using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFramework.Samples.PersistentData
{
    public class RandomMove : MonoBehaviour
    {
        public float moveSpeed = 1;
        public float moveRadius = 10f;
        [SerializeField] [Flag]
        private FreezeAxis freezeAxis;
        [SerializeField]
        private Vector3 targetPos;

        private void Start()
        {
            targetPos = GetRandomPos();
        }

        private void Update()
        {
            if (VectorUtils.Approximately(transform.position, targetPos))
            {
                targetPos = GetRandomPos();
                transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                transform.localScale = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            }
        }

        private Vector3 GetRandomPos()
        {
            Vector3 pos = Random.insideUnitSphere * moveRadius;
            if ((freezeAxis & FreezeAxis.FreezeXAxis) == FreezeAxis.FreezeXAxis)
            {
                pos.x = 0;
            }

            if ((freezeAxis & FreezeAxis.FreezeYAxis) == FreezeAxis.FreezeYAxis)
            {
                pos.y = 0;
            }

            if ((freezeAxis & FreezeAxis.FreezeZAxis) == FreezeAxis.FreezeZAxis)
            {
                pos.z = 0;
            }

            return pos;
        }

        [Flags]
        private enum FreezeAxis
        {
            FreezeXAxis = 1,
            FreezeYAxis = 2,
            FreezeZAxis = 4
        }
    }
}