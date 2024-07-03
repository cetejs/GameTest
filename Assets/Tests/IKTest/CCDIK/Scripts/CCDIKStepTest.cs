using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDIKStepTest : MonoBehaviour
{
    public Transform effector;
    public Transform baseBone;
    public Transform target;
    public float sqrDistanceError = 0.001f;
    public int maxIterationCount = 10;
    public float waitSecond = 1;
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
        float sqrDistance;
        int iterationCount = 0;

        do
        {
            for (int i = 1; i < bones.Count; i++)
            {
                Vector3 effectorPos = effector.position;
                Vector3 bonePos = bones[i].position;
                Debug.DrawLine(bonePos, effectorPos, Color.red, waitSecond);
                Debug.DrawLine(bonePos, target.position, Color.green, waitSecond);
                yield return new WaitForSeconds(waitSecond);
                bones[i].rotation = Quaternion.FromToRotation(effectorPos - bonePos, target.position - bonePos) * bones[i].rotation;
                sqrDistance = (effectorPos - target.position).sqrMagnitude;
                if (sqrDistance <= sqrDistanceError)
                {
                    yield break;
                }

                yield return new WaitForSeconds(waitSecond);
            }

            sqrDistance = (effector.position - target.position).sqrMagnitude;
            iterationCount++;
        }
        while (sqrDistance > sqrDistanceError && iterationCount <= maxIterationCount);
    }
}
