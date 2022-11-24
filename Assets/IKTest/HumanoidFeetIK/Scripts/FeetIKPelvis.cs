using System;
using UnityEngine;

public class FeetIKPelvis
{
    private PelvisInfo info;
    private Animator anim;
    private Transform transform;
    private float lastTime;
    private float pelvisOffset;

    public FeetIKPelvis(PelvisInfo info, Animator anim)
    {
        this.info = info;
        this.anim = anim;
    }

    public FeetIKPelvis(PelvisInfo info)
    {
        this.info = info;
        transform = info.pelvis;
    }

    public void Process(float lowestOffset, float highestOffset, bool isGrounded)
    {
        float deltaTime = Time.time - lastTime;
        if (deltaTime == 0.0f)
        {
            return;
        }

        lastTime = Time.time;
        lowestOffset *= info.lowerPelvisWeight;
        highestOffset *= info.liftPelvisWeight;
        float offsetTarget = lowestOffset + highestOffset;
        if (!isGrounded)
        {
            offsetTarget = 0.0f;
        }

        pelvisOffset = Mathf.Lerp(pelvisOffset, offsetTarget, info.pelvisSpeed * deltaTime);

        if (anim)
        {
            anim.bodyPosition -= Vector3.up * pelvisOffset;
        }
        else
        {
            transform.position -= Vector3.up * pelvisOffset;
        }
    }
}

[Serializable]
public class PelvisInfo
{
    public Transform pelvis;
    [Range(0.0f, 1.0f)]
    public float lowerPelvisWeight = 1.0f;
    [Range(0.0f, 1.0f)]
    public float liftPelvisWeight;
    public float pelvisSpeed = 5.0f;
}