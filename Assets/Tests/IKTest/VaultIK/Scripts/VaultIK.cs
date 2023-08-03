using GameFramework;
using UnityEngine;

public class VaultIK : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private string vaultHash = "Vault";
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    private float raycastHeightFromRoot = 0.3f;
    [SerializeField]
    private float raycastMaxDistance = 5f;

    private AnimatorParameter vaultParameter;
    private Vector3 matchTarget;
    private VaultIKSMB smb;

    private void Awake()
    {
        vaultParameter = new AnimatorParameter(anim, vaultHash);
        smb = anim.GetBehaviour<VaultIKSMB>();
    }

    public bool Vault()
    {
        Ray ray = new Ray(anim.rootPosition + Vector3.up * raycastHeightFromRoot, anim.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, raycastMaxDistance, obstacleMask))
        {
            return false;
        }

        if (!vaultParameter.IsValid)
        {
            return false;
        }

        matchTarget = hitInfo.point;
        Bounds bounds = hitInfo.collider.bounds;
        matchTarget.y = bounds.center.y + bounds.size.y / 2.0f;
        smb.SetMatchTarget(matchTarget);
        anim.SetTrigger(vaultParameter);
        return true;
    }
}