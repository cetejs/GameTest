using GameFramework;
using UnityEngine;

public class SimpleFollowTarget : FollowTargetCamera
{
    [SerializeField]
    private float moveSpeed = 5.0f;
    private Vector3 offset;
    
    protected override void Awake()
    {
        offset = transform.position - target.position;
    }

    protected override void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
    }
}