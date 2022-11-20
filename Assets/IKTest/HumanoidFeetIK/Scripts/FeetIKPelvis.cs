using System;
using UnityEngine;

    public class FeetIKPelvis
    {
        private PelvisInfo info;
        private Animator anim;
        private float lastTime;
        private float pelvisOffset;

        public FeetIKPelvis(PelvisInfo info, Animator anim)
        {
            this.info = info;
            this.anim = anim;
        }

        public void Process(float lowestOffset, float highestOffset, bool isGrounded)
        {
            float deltaTime = Time.time - lastTime;
            if (deltaTime == 0.0f)
            {
                return;
            }

            lastTime = Time.time;
            float offsetTarget = lowestOffset + highestOffset;
            if (!isGrounded)
            {
                offsetTarget = 0.0f;
            }

            pelvisOffset = Mathf.Lerp(pelvisOffset, offsetTarget, info.pelvisSpeed * deltaTime);
            anim.bodyPosition -= Vector3.up * pelvisOffset;
        }
    }
    
    [Serializable]
    public class PelvisInfo
    {
        public float lowerPelvisWeight = 1.0f;
        public float liftPelvisWeight;
        public float pelvisSpeed = 7.0f;
    }
