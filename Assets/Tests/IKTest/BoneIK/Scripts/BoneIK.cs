using GameFramework;
using UnityEngine;

public class BoneIK : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform trackTarget;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float boneWeight = 1.0f;
    [SerializeField]
    private float matchSpeed = 1.0f;
    [SerializeField]
    private bool isIKPosition = true;
    [SerializeField]
    private bool isIKRotation = true;
    [SerializeField]
    private AvatarIKGoal avatarIKGoal = AvatarIKGoal.LeftHand;
    [SerializeField]
    private string boneIKCurve = "BoneIK";

    private AnimatorParameter boneParameter;
    private float weight;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        boneParameter = new AnimatorParameter(anim, boneIKCurve);
    }

    private void Update()
    {
        if (!anim)
        {
            return;
        }

        weight = Mathf.Lerp(weight, boneWeight, matchSpeed * Time.deltaTime);

        if (boneParameter.IsValid)
        {
            weight = Mathf.Clamp(weight, 0.0f, anim.GetFloat(boneParameter));
        }
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

        if (isIKPosition)
        {
            anim.SetIKPositionWeight(avatarIKGoal, weight);
            anim.SetIKPosition(avatarIKGoal, trackTarget.position);
        }

        if (isIKRotation)
        {
            anim.SetIKRotationWeight(avatarIKGoal, weight);
            anim.SetIKRotation(avatarIKGoal, trackTarget.rotation);
        }
    }

    public void SetIKTarget(Transform target)
    {
        trackTarget = target;
    }
}