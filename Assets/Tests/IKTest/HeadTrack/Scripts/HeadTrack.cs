using GameFramework;
using UnityEngine;

public class HeadTrack : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform trackTarget;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float lookAtWight = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float bodyWeight = 0.5f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float headWeight = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float eyesWeight = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float clampWeight = 0.5f;
    [SerializeField]
    private float maxDistance = 3.0f;
    [SerializeField]
    private float lookSpeed = 1.0f;

    private Transform rawTarget;
    private Transform head;
    private float weight;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        head = anim.GetBoneTransform(HumanBodyBones.Head);
    }

    private void Update()
    {
        if (!anim)
        {
            return;
        }

        if (rawTarget)
        {
            trackTarget.position = rawTarget.position;
        }

        bool isOutRange = !trackTarget || Vector3.SqrMagnitude(head.position - trackTarget.position) > maxDistance * maxDistance;
        weight = Mathf.Lerp(weight, isOutRange ? 0.0f : lookAtWight, lookSpeed * Time.deltaTime);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!anim || !trackTarget)
        {
            return;
        }

        if (Mathf.Abs(weight) < 0.01f)
        {
            return;
        }

        anim.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
        anim.SetLookAtPosition(trackTarget.position);
    }

    public void LookAtPosition(Vector3 pos)
    {
        trackTarget.position = pos;
    }

    public void LookAtTransform(Transform target)
    {
        rawTarget = target;
    }
}