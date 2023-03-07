using UnityEngine;

public class FABRIKSolver
{
    private int maxIterationCount;
    private float sqrDistanceError;
    private float posIKWeight;
    private float rotIKWeight;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3[] bonePositions;
    private Vector3[] boneStartDirs;
    private Quaternion[] boneStartRots;
    private float[] boneLengths;
    private float completeLength;
    private IKPoleBones bones;
    private Transform root;

    public FABRIKSolver()
    {
    }

    public FABRIKSolver(IKPoleBones bones, int maxIterationCount = 10, float distanceError = 0.001f)
    {
        Init(bones, maxIterationCount, distanceError);
    }

    public void Init(IKPoleBones bones, int maxIterationCount = 10, float distanceError = 0.001f)
    {
        bones.Update();
        this.bones = bones;
        this.maxIterationCount = maxIterationCount;
        sqrDistanceError = distanceError * distanceError;

        root = bones.baseBone.parent ? bones.baseBone.parent : bones.baseBone;
        bonePositions = new Vector3[bones.Count];
        boneStartDirs = new Vector3[bones.Count - 1];
        boneLengths = new float[bones.Count - 1];
        boneStartRots = new Quaternion[bones.Count];

        for (int i = 0; i < bones.Count; i++)
        {
            if (i > 0)
            {
                int j = i - 1;
                boneStartDirs[j] = GetRootSpacePosition(bones[j].position) - GetRootSpacePosition(bones[i].position);
                boneLengths[j] = boneStartDirs[j].magnitude;
                completeLength += boneLengths[j];
            }

            boneStartRots[i] = GetRootSpaceRotation(bones[i].rotation);
        }
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
        int iterationCount = 0;

        for (int i = 0; i < bonePositions.Length; i++)
        {
            bonePositions[i] = GetRootSpacePosition(bones[i].position);
        }

        Vector3 effectorPos = bonePositions[0];
        Vector3 baseBonePos = bonePositions[bonePositions.Length - 1];
        Vector3 targetPos = GetRootSpacePosition(targetPosition);
        if (posIKWeight < 1)
        {
            targetPos = Vector3.Lerp(effectorPos, targetPos, posIKWeight);
        }

        float baseToTargetLength = (baseBonePos - targetPos).sqrMagnitude;
        if (baseToTargetLength >= completeLength * completeLength)
        {
            Vector3 forward = (targetPos - baseBonePos).normalized;
            for (int i = bones.Count - 2; i >= 0; i--)
            {
                bonePositions[i] = bonePositions[i + 1] + forward * boneLengths[i];
            }
        }
        else
        {
            float sqrDistance;
            do
            {
                for (int i = 0; i < bonePositions.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        bonePositions[i] = targetPos;
                    }
                    else
                    {
                        int j = i - 1;
                        bonePositions[i] = bonePositions[j] + (bonePositions[i] - bonePositions[j]).normalized * boneLengths[j];
                    }
                }

                for (int i = bonePositions.Length - 2; i >= 0; i--)
                {
                    int j = i + 1;
                    bonePositions[i] = bonePositions[j] + (bonePositions[i] - bonePositions[j]).normalized * boneLengths[i];
                }

                effectorPos = bonePositions[0];
                sqrDistance = (effectorPos - targetPos).sqrMagnitude;
                iterationCount++;
            }
            while (sqrDistance > sqrDistanceError && iterationCount <= maxIterationCount);
        }

        if (bones.pole)
        {
            Vector3 polePos = GetRootSpacePosition(bones.pole.position);
            for (int i = bonePositions.Length - 2; i > 0; i--)
            {
                int j = i + 1, k = i - 1;
                Plane plane = new Plane(bonePositions[k] - bonePositions[j], bonePositions[j]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(polePos);
                Vector3 projectedBone = plane.ClosestPointOnPlane(bonePositions[i]);
                float angle = Vector3.SignedAngle(projectedBone - bonePositions[j], projectedPole - bonePositions[j], plane.normal);
                bonePositions[i] = Quaternion.AngleAxis(angle, plane.normal) * (bonePositions[i] - bonePositions[j]) + bonePositions[j];
            }
        }

        for (int i = bones.Count - 1; i >= 0; i--)
        {
            if (i > 0)
            {
                int j = i - 1;
                SetRootSpaceRotation(bones[i], Quaternion.FromToRotation(boneStartDirs[j], bonePositions[j] - bonePositions[i]) * boneStartRots[i]);
            }

            SetRootSpacePosition(bones[i], bonePositions[i]);
        }
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

    private Vector3 GetRootSpacePosition(Vector3 worldPos)
    {
        if (root)
        {
            return Quaternion.Inverse(root.rotation) * (worldPos - root.position);
        }

        return worldPos;
    }

    private void SetRootSpacePosition(Transform bone, Vector3 localPos)
    {
        if (root)
        {
            bone.position = root.rotation * localPos + root.position;
        }
        else
        {
            bone.position = localPos;
        }
    }

    private Quaternion GetRootSpaceRotation(Quaternion worldRot)
    {
        if (root)
        {
            return Quaternion.Inverse(root.rotation) * worldRot;
        }

        return worldRot;
    }

    private void SetRootSpaceRotation(Transform bone, Quaternion localRot)
    {
        if (root)
        {
            bone.rotation = root.rotation * localRot;
        }
        else
        {
            bone.rotation = localRot;
        }
    }
}