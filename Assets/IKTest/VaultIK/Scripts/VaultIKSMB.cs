using UnityEngine;
using UnityEngine.Animations;

public class VaultIKSMB : StateMachineBehaviour
{
    [SerializeField]
    private float handHeight = 0.1f;
    private Vector3 matchTarget;
    private MatchTargetWeightMask mask;

    private void Awake()
    {
        mask = new MatchTargetWeightMask(Vector3.one, 0);
    }

    public void SetMatchTarget(Vector3 position)
    {
        matchTarget = position;
        matchTarget.y += handHeight;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (animator.IsInTransition(layerIndex))
        {
            return;
        }

        animator.MatchTarget(matchTarget, Quaternion.identity, AvatarTarget.LeftHand, mask, 0.2f, 0.4f);
    }
}