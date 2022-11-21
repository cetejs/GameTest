using UnityEngine;

public class HumanoidFeetIK : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float widget = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float footRotationWeight = 1;
    [SerializeField]
    private FootIKInfo footIKInfo;
    [SerializeField]
    private PelvisInfo pelvisInfo;

    private FootIKSolver leftFootSolver, rightFootSolver;
    private FeetIKPelvis pelvis;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        Transform leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        Transform rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        footIKInfo.root = anim.transform;
        leftFootSolver = new FootIKSolver(footIKInfo, leftFoot);
        rightFootSolver = new FootIKSolver(footIKInfo, rightFoot);
        pelvis = new FeetIKPelvis(pelvisInfo, anim);
    }

    private void OnValidate()
    {
        footIKInfo.root = anim? anim.transform : null;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!anim || widget <= 0)
        {
            return;
        }

        leftFootSolver.Process();
        rightFootSolver.Process();
        MovePelvisHeight();
        AppleFootIk(AvatarIKGoal.LeftFoot, leftFootSolver.IKPosition, leftFootSolver.IKRotation);
        AppleFootIk(AvatarIKGoal.RightFoot, rightFootSolver.IKPosition, rightFootSolver.IKRotation);
    }

    private void OnDrawGizmos()
    {
        if (leftFootSolver != null)
        {
            leftFootSolver.OnDrawGizmos();
        }

        if (rightFootSolver != null)
        {
            rightFootSolver.OnDrawGizmos();
        }
    }

    private void MovePelvisHeight()
    {
        bool isGrounded = leftFootSolver.IsGrounded || rightFootSolver.IsGrounded;
        float lowestOffset = Mathf.Max(leftFootSolver.IKOffset, rightFootSolver.IKOffset);
        float highestOffset = Mathf.Min(leftFootSolver.IKOffset, rightFootSolver.IKOffset);
        lowestOffset = Mathf.Max(lowestOffset, 0f);
        highestOffset = Mathf.Min(highestOffset, 0f);
        pelvis.Process(lowestOffset , highestOffset, isGrounded);
    }

    private void AppleFootIk(AvatarIKGoal foot, Vector3 footIKPos, Quaternion footIKRot)
    {
        anim.SetIKPositionWeight(foot, widget);
        anim.SetIKRotationWeight(foot, widget * footRotationWeight);
        anim.SetIKPosition(foot, footIKPos);
        anim.SetIKRotation(foot, footIKRot);
    }
}