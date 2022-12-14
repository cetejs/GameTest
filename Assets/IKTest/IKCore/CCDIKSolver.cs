using UnityEngine;

public class CCDIKSolver
{
    private int maxIterationCount;
    private float sqrDistanceError;
    private float posIKWeight;
    private float rotIKWeight;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private IKBones bones;

    public CCDIKSolver()
    {
    }

    public CCDIKSolver(IKBones bones, int maxIterationCount = 10, float distanceError = 0.001f)
    {
        Init(bones, maxIterationCount, distanceError);
    }

    public void Init(IKBones bones, int maxIterationCount = 10, float distanceError = 0.001f)
    {
        bones.Update();
        this.bones = bones;
        this.maxIterationCount = maxIterationCount;
        sqrDistanceError = distanceError * distanceError;
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
        targetPosition = pos;
    }

    public void SetIKRotation(Quaternion rot)
    {
        targetRotation = rot;
    }

    public void Process()
    {
        ProcessPosition();
        ProcessRotation();
    }

    private void ProcessPosition()
    {
        Transform effector = bones.effector;
        Vector3 targetPos;
        if (posIKWeight >= 1)
        {
            targetPos = targetPosition;
        }
        else
        {
            targetPos = Vector3.Lerp(effector.position, targetPosition, posIKWeight);
        }

        float sqrDistance;
        int iterationCount = 0;

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

    private void ProcessRotation()
    {
        Transform effector = bones.effector;
        Quaternion boneRot = effector.rotation;
        if (rotIKWeight >= 1)
        {
            effector.rotation = targetRotation;
        }
        else
        {
            effector.rotation = Quaternion.Lerp(boneRot, targetRotation, rotIKWeight);
        }
    }
}