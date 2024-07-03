using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABRIKStepTest : MonoBehaviour
{
    public Transform effector;
    public Transform baseBone;
    public Transform target;
    public float sqrDistanceError = 0.001f;
    public int maxIterationCount = 10;
    public float waitSecond = 1;
    private Transform root;
    private Vector3[] bonePositions;
    private Vector3[] boneStartDirs;
    private Quaternion[] boneStartRots;
    private float[] boneLengths;
    private float completeLength;
    private List<Transform> bones = new List<Transform>();

    private IEnumerator enumerator;
    
    private void Start()
    {
        bones.Clear();
        Transform current = effector;
        while (current != null && current != baseBone.parent)
        {
            bones.Add(current);
            current = current.parent;
        }

        if (current == null)
        {
            bones.Clear();
        }
        
        root = baseBone.parent ? baseBone.parent : baseBone;
        bonePositions = new Vector3[bones.Count];
        boneStartDirs = new Vector3[bones.Count - 1];
        boneStartRots = new Quaternion[bones.Count];
        boneLengths = new float[bones.Count - 1];
        
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

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StopAllCoroutines();
            StartCoroutine(ProcessCCDIK());
        }
    }

    private IEnumerator ProcessCCDIK()
    {
        int iterationCount = 0;

        for (int i = 0; i < bonePositions.Length; i++)
        {
            bonePositions[i] = GetRootSpacePosition(bones[i].position);
        }

        Vector3 effectorPos = bonePositions[0];
        Vector3 baseBonePos = bonePositions[bonePositions.Length - 1];
        Vector3 targetPos = GetRootSpacePosition(target.position);

        float baseToTargetLength = (baseBonePos - targetPos).sqrMagnitude;
        if (baseToTargetLength >= completeLength * completeLength)
        {
            Vector3 forward = (targetPos - baseBonePos).normalized;
            for (int i = bones.Count - 2; i >= 0; i--)
            {
                bonePositions[i] = bonePositions[i + 1] + forward * boneLengths[i];
                yield return new WaitForSeconds(waitSecond);
                SetRootSpacePosition(bones[i], bonePositions[i]);
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
                        Debug.DrawLine(bones[i].position, bonePositions[i], Color.red, waitSecond);
                        yield return new WaitForSeconds(waitSecond);
                        SetRootSpacePosition(bones[i], bonePositions[i]);
                        yield return new WaitForSeconds(waitSecond);
                    }
                    else
                    {
                        int j = i - 1;
                        bonePositions[i] = bonePositions[j] + (bonePositions[i] - bonePositions[j]).normalized * boneLengths[j];
                        Debug.DrawLine(bonePositions[j], bonePositions[i], Color.green, waitSecond);
                        Debug.DrawLine(bones[i].position, bonePositions[i], Color.red, waitSecond);
                        yield return new WaitForSeconds(waitSecond);
                        SetRootSpacePosition(bones[i], bonePositions[i]);
                        yield return new WaitForSeconds(waitSecond);
                    }
                }

                for (int i = bonePositions.Length - 2; i >= 0; i--)
                {
                    int j = i + 1;
                    bonePositions[i] = bonePositions[j] + (bonePositions[i] - bonePositions[j]).normalized * boneLengths[i];
                    Debug.DrawLine(bonePositions[j], bonePositions[i], Color.green, waitSecond);
                    Debug.DrawLine(bones[i].position, bonePositions[i], Color.red, waitSecond);
                    yield return new WaitForSeconds(waitSecond);
                    SetRootSpacePosition(bones[i], bonePositions[i]);
                    yield return new WaitForSeconds(waitSecond);
                }

                effectorPos = bonePositions[0];
                sqrDistance = (effectorPos - targetPos).sqrMagnitude;
                iterationCount++;
            }
            while (sqrDistance > sqrDistanceError && iterationCount <= maxIterationCount);
            
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
    }
    
    // 把世界空间下的坐标转换成 Root 下的本地坐标
    private Vector3 GetRootSpacePosition(Vector3 worldPos)
    {
        return Quaternion.Inverse(root.rotation) * (worldPos - root.position);
    }

    private void SetRootSpacePosition(Transform bone, Vector3 localPos)
    {
        
        bone.position = root.rotation * localPos + root.position;
    }
    
    private Quaternion GetRootSpaceRotation(Quaternion worldRot)
    {
        
        return Quaternion.Inverse(root.rotation) * worldRot;
    }

    private void SetRootSpaceRotation(Transform bone, Quaternion localRot)
    {
       
        bone.rotation = root.rotation * localRot;
    }
}
