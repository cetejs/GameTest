using GameFramework.DelayedActionService;
using GameFramework.Generic;
using UnityEngine;

public class VaultIKTest : MonoBehaviour
{
    [SerializeField]
    private VaultIK vaultIK;
    [SerializeField]
    private Vector3 origin;
    [SerializeField]
    private float loopTime = 2.5f;

    private bool canVault = true;

    private void Update()
    {
        if (canVault && vaultIK.Vault())
        {
            canVault = false;
            Global.GetService<DelayedActionManager>().AddAction(() =>
            {
                canVault = true;
                transform.position = origin;
            }, loopTime);
        }
    }
}