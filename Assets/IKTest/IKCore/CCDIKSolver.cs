using UnityEngine;

public class CCDIKSolver
{
    private float sqrDistanceError;
    private int maxIterationCount;
    private float posIKWeight;
    private float rotIKWeight;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private IKBones bones;

    public Transform Effector
    {
        get { return bones.effector; }
    }

    public CCDIKSolver()
    {
    }

    public CCDIKSolver(IKBones bones, float sqrDistanceError = 1e-6f, int maxIterationCount = 10)
    {
        Init(bones, sqrDistanceError, maxIterationCount);
    }

    public void Init(IKBones bones, float sqrDistanceError = 1e-6f, int maxIterationCount = 10)
    {
        if (bones.Count == 0)
        {
            bones.Update();
        }

        this.bones = bones;
        this.sqrDistanceError = sqrDistanceError;
        this.maxIterationCount = maxIterationCount;
    }

    public void SetIKPositionWeight(float weight)
    {
        posIKWeight = weight;
    }

    public void SetIKRotationWeight(float weight)
    {
        rotIKWeight = weight;
    }

    public void SetIKPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    public void SetIKRotation(Quaternion rot)
    {
        targetRot = rot;
    }

    public void Process()
    {
        Transform effector = bones.effector;
        Vector3 targetPos = Vector3.Lerp(effector.position, this.targetPos, posIKWeight);
        float sqrDistance;
        int iterationCount = 0;

        Quaternion boneRot = effector.rotation;
        effector.rotation = Quaternion.Lerp(boneRot, targetRot, rotIKWeight);

        do
        {
            for (int i = 0; i < bones.Count - 2; i++)
            {
                for (int j = 1; j < i + 3 && j < bones.Count; j++)
                {
                    Vector3 effectorPos = effector.position;
                    Vector3 bonePos = bones[j].position;
                    bones[j].rotation = Quaternion.FromToRotation(effectorPos - bonePos, targetPos - bonePos) * bones[j].rotation;
                    sqrDistance = (effectorPos - targetPos).sqrMagnitude;

                    if (sqrDistance <= sqrDistanceError)
                    {
                        return;
                    }
                }
            }

            sqrDistance = (effector.position - targetPos).sqrMagnitude;
            iterationCount++;
        }
        while (sqrDistance > sqrDistanceError && iterationCount <= maxIterationCount);
    }
}