using GameFramework;
using UnityEngine;

public class LegacyFeetIK : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private LayerMask environmentLayer;
    [SerializeField]
    private string leftFootIKCurve = "LeftFootIK";
    [SerializeField]
    private string rightFootIKCurve = "RightFootIK";
    [SerializeField]
    private bool isUseIKRotate = true;
    [SerializeField]
    private bool isMovePelvis = true;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float pelvisMoveSpeed = 0.5f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float footIKPosSpeed = 0.5f;
    [SerializeField] [Range(0.0f, 2.0f)]
    private float raycastUpHeight = 0.5f;
    [SerializeField] [Range(0.0f, 2.0f)]
    private float raycastDownDistance = 0.5f;
    [SerializeField] [Range(-180, 180)]
    private float leftFootAngleOffset;
    [SerializeField] [Range(-180, 180)]
    private float rightFootAngleOffset;

    private AnimatorParameter leftParameter, rightParameter;
    private Transform leftFoot, rightFoot;
    private Vector3 leftFootIKPos, rightFootIKPos;
    private Quaternion leftFooIKRot, rightFootIKRot;
    private float leftFootHeight, rightFootHeight;
    private float lastPelvisPosY, lastLeftFootPosY, lastRightFootPosY;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        leftParameter = new AnimatorParameter(anim, leftFootIKCurve);
        rightParameter = new AnimatorParameter(anim, rightFootIKCurve);
        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);

        leftFootHeight = leftFoot.position.y - transform.position.y;
        rightFootHeight = rightFoot.position.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!anim)
        {
            return;
        }

        SolveFootIK(leftFoot, leftFootAngleOffset, out leftFootIKPos, out leftFooIKRot);
        SolveFootIK(rightFoot, rightFootAngleOffset, out rightFootIKPos, out rightFootIKRot);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!anim)
        {
            return;
        }

        MovePelvisHeight();
        SetIKWeight(AvatarIKGoal.LeftFoot, leftParameter);
        SetIKWeight(AvatarIKGoal.RightFoot, rightParameter);
        AppleFootIk(AvatarIKGoal.LeftFoot, leftFootIKPos, leftFooIKRot, leftFootHeight, ref lastLeftFootPosY);
        AppleFootIk(AvatarIKGoal.RightFoot, rightFootIKPos, rightFootIKRot, rightFootHeight, ref lastRightFootPosY);
    }

    private void SolveFootIK(Transform foot, float angleOffset, out Vector3 footIKPos, out Quaternion footIKRot)
    {
        Ray ray = new Ray(foot.position + Vector3.up * raycastUpHeight, Vector3.down);
        float maxDistance = raycastUpHeight + raycastDownDistance;

#if UNITY_EDITOR
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.red);
#endif

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, environmentLayer, QueryTriggerInteraction.Ignore))
        {
            footIKPos = foot.position;
            footIKPos.y = hitInfo.point.y;
            footIKRot = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * transform.rotation;
            footIKRot *= Quaternion.AngleAxis(angleOffset, Vector3.up);
        }
        else
        {
            footIKPos = Vector3.zero;
            footIKRot = transform.rotation;
        }
    }

    private void SetIKWeight(AvatarIKGoal foot, AnimatorParameter parameter)
    {
        if (parameter.IsValid)
        {
            anim.SetIKPositionWeight(foot, anim.GetFloat(parameter));
            if (isUseIKRotate)
            {
                anim.SetIKRotationWeight(foot, anim.GetFloat(parameter));
            }
        }
    }

    private void MovePelvisHeight()
    {
        if (!isMovePelvis)
        {
            return;
        }

        if (leftFootIKPos == Vector3.zero || rightFootIKPos == Vector3.zero || lastPelvisPosY == 0.0f)
        {
            lastPelvisPosY = anim.bodyPosition.y;
            return;
        }

        float leftOffsetPos = leftFootIKPos.y - transform.position.y;
        float rightOffsetPos = rightFootIKPos.y - transform.position.y;
        float offsetPos = Mathf.Min(leftOffsetPos, rightOffsetPos);
        Vector3 pelvisPos = anim.bodyPosition + Vector3.up * offsetPos;
        pelvisPos.y = Mathf.Lerp(lastPelvisPosY, pelvisPos.y, pelvisMoveSpeed);
        anim.bodyPosition = pelvisPos;
        lastPelvisPosY = pelvisPos.y;
    }

    private void AppleFootIk(AvatarIKGoal foot, Vector3 footIKPos, Quaternion footIKRot, float footHeight, ref float lastFootPosY)
    {
        Vector3 targetIkPos = anim.GetIKPosition(foot);
        if (footIKPos != Vector3.zero)
        {
            targetIkPos.y = Mathf.Lerp(lastFootPosY, footIKPos.y + footHeight, footIKPosSpeed);
            lastFootPosY = targetIkPos.y;
        }

        anim.SetIKPosition(foot, targetIkPos);
        anim.SetIKRotation(foot, footIKRot);
    }
}