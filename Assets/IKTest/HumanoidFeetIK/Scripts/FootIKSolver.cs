using System;
using UnityEngine;

public class FootIKSolver
{
    private FootIKInfo info;
    private Transform transform;
    private float ikOffset;
    private Quaternion rotationOffset;
    private Vector3 position;
    private Vector3 lastPosition;
    private Vector3 velocity;
    private float lastTime;
    private bool isGrounded;

    private RaycastHit hitInfo;

    public Vector3 IKPosition
    {
        get { return position - Vector3.up * ikOffset; }
    }

    public Quaternion IKRotation
    {
        get { return transform.rotation * rotationOffset; }
    }

    public float IKOffset
    {
        get { return ikOffset; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    public FootIKSolver(FootIKInfo info, Transform transform)
    {
        this.info = info;
        this.transform = transform;
    }

    public void Process()
    {
        float deltaTime = Time.time - lastTime;
        if (deltaTime == 0.0f)
        {
            return;
        }

        lastTime = Time.time;
        position = transform.position;
        velocity = (position - lastPosition) / deltaTime;
        lastPosition = position;

        Vector3 prediction = position + velocity * info.prediction;
        float rootYOffset = position.y - info.root.position.y + info.heightOffset;
        hitInfo = Raycast(prediction);
        if (info.quality == FootIKInfo.Quality.Sample)
        {
            RaycastHit footHit = Raycast(position);
            if (hitInfo.point.y < footHit.point.y)
            {
                hitInfo = footHit;
            }
        }

        isGrounded = hitInfo.collider;
        float heightFormGround = position.y - hitInfo.point.y - rootYOffset;
        float offsetTarget = Mathf.Clamp(heightFormGround, -info.maxStep, info.maxStep);
        if (!isGrounded)
        {
            offsetTarget = 0.0f;
        }

        ikOffset = Mathf.MoveTowards(ikOffset, offsetTarget, info.footSpeed * deltaTime);

        Vector3 normal = Vector3.RotateTowards(Vector3.up, hitInfo.normal, info.maxFootRotationAngle * Mathf.Deg2Rad, deltaTime);
        Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, normal);
        Quaternion rotationOffsetTarget = Quaternion.RotateTowards(Quaternion.identity, hitRotation, info.maxFootRotationAngle);
        rotationOffset = Quaternion.Slerp(rotationOffset, rotationOffsetTarget, info.footRotationSpeed * deltaTime);
    }

    public void OnDrawGizmos()
    {
        if (!info.isDebugInfo)
        {
            return;
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitInfo.point, 0.05f);
    }

    private RaycastHit Raycast(Vector3 origin)
    {
        Ray ray = new Ray(origin + info.maxStep * Vector3.up, Vector3.down);
        float maxDistance = info.maxStep * 2.0f;

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, info.environmentLayer, QueryTriggerInteraction.Ignore))
        {
            hitInfo.point = origin;
            hitInfo.normal = Vector3.up;
        }

#if UNITY_EDITOR
        if (info.isDebugInfo)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.red);
        }
#endif
        return hitInfo;
    }
}

[Serializable]
public class FootIKInfo
{
    public Transform root;
    public LayerMask environmentLayer;
    public float heightOffset;
    public float footSpeed = 2.5f;
    public float footRotationSpeed = 7;
    public float prediction = 0.05f;
    public float maxStep = 0.5f;
    public float maxFootRotationAngle = 45.0f;
    public Quality quality = Quality.Sample;
    public bool isDebugInfo;

    public enum Quality
    {
        Faster,
        Sample
    }
}