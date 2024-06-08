using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GameFramework.Samples.PersistentData
{
    public class TransformSave : PersistentSave<TransformData>
    {
        private void Awake()
        {
            PersistentData = new TransformData()
            {
                position = new CustomVector3(Vector3.zero),
                eulerAngles = new CustomVector3(Vector3.zero),
                localScale = new CustomVector3(Vector3.one)
            };
        }

        protected override void OnGetPersistentData()
        {
            transform.position = PersistentData.position.ToVector3();
            transform.eulerAngles = PersistentData.eulerAngles.ToVector3();
            transform.localScale = PersistentData.localScale.ToVector3();
        }

        protected override void OnSavePersistentData()
        {
            PersistentData = new TransformData()
            {
                position = new CustomVector3(transform.position),
                eulerAngles = new CustomVector3(transform.eulerAngles),
                localScale = new CustomVector3(transform.localScale)
            };
        }
    }

    [Serializable]
    public struct TransformData // 里面存放了位置旋转缩放的数据
    {
        public CustomVector3 position;
        public CustomVector3 eulerAngles;
        public CustomVector3 localScale;
    }

    [Serializable]
    public struct CustomVector3
    {
        public float x;
        public float y;
        public float z;

        public CustomVector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3()
            {
                x = x,
                y = y,
                z = z
            };
        }
    }
}