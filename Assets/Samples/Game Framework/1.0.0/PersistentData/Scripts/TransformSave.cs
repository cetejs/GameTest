using UnityEngine;

namespace GameFramework
{
    public class TransformSave : PersistentSave<TransformData>
    {
        private void Awake()
        {
            PersistentData = new TransformData()
            {
                position = Vector3.zero,
                eulerAngles = Vector3.zero,
                localScale = Vector3.one
            };
        }

        protected override void OnGetPersistentData()
        {
            transform.position = PersistentData.position;
            transform.eulerAngles = PersistentData.eulerAngles;
            transform.localScale = PersistentData.localScale;
        }

        protected override void OnSavePersistentData()
        {
            PersistentData = new TransformData()
            {
                position = transform.position,
                eulerAngles = transform.eulerAngles,
                localScale = transform.localScale
            };
        }
    }

    public struct TransformData
    {
        public Vector3 position;
        public Vector3 eulerAngles;
        public Vector3 localScale;
    }
}