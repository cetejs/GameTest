using UnityEngine;

public class CCDIK : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)]
    private float weight = 1.0f;
    [SerializeField][Range(0.0f, 1.0f)]
    private float rotationWeight = 1.0f;
    [SerializeField]
    private IKBones bones;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float sqrDistanceError = 1e-6f;
    [SerializeField]
    private int maxIterationCount = 10;
    private CCDIKSolver solver = new CCDIKSolver();

    private void Awake()
    {
        solver.Init(bones, sqrDistanceError, maxIterationCount);
    }

    private void LateUpdate()
    {
        solver.SetIKPositionWeight(weight);
        solver.SetIKRotationWeight(rotationWeight);
        solver.SetIKPosition(target.position);
        solver.SetIKRotation(target.rotation);
        solver.Process();
    }
}
