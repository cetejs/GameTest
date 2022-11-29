using System.Collections.Generic;
using UnityEngine;

public class FeetIK : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float weight = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float footRotationWeight = 1;
    [SerializeField]
    private FootIKInfo footIKInfo;
    [SerializeField]
    private PelvisInfo pelvisInfo;
    [SerializeField]
    private List<IKPoleBones> bones = new List<IKPoleBones>();

    private FootIKSolver[] footIKSolvers;
    private FABRIKSolver[] fabrIKSolvers;
    private FeetIKPelvis pelvis;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        footIKInfo.root = anim.transform;
        footIKSolvers = new FootIKSolver[bones.Count];
        fabrIKSolvers = new FABRIKSolver[bones.Count];
        pelvis = new FeetIKPelvis(pelvisInfo);
        for (int i = 0; i < bones.Count; i++)
        {
            footIKSolvers[i] = new FootIKSolver(footIKInfo, bones[i].effector);
            fabrIKSolvers[i] = new FABRIKSolver(bones[i]);
        }
    }

    private void OnValidate()
    {
        if (footIKInfo != null)
        {
            footIKInfo.root = anim ? anim.transform : null;
        }
    }

    private void LateUpdate()
    {
        if (!anim || weight <= 0)
        {
            return;
        }

        for (int i = 0; i < bones.Count; i++)
        {
            footIKSolvers[i].Process();
        }

        MovePelvisHeight();
        for (int i = 0; i < bones.Count; i++)
        {
            fabrIKSolvers[i].SetIKPositionWeight(weight);
            fabrIKSolvers[i].SetIKRotationWeight(weight * footRotationWeight);
            fabrIKSolvers[i].SetIKPosition(footIKSolvers[i].IKPosition);
            fabrIKSolvers[i].SetIKRotation(footIKSolvers[i].IKRotation);
            fabrIKSolvers[i].Process();
        }
    }

    private void OnDrawGizmos()
    {
        if (footIKSolvers == null)
        {
            return;
        }

        foreach (FootIKSolver solver in footIKSolvers)
        {
            solver.OnDrawGizmos();
        }
    }

    private void MovePelvisHeight()
    {
        bool isGrounded = false;
        float lowestOffset = float.MinValue;
        float highestOffset = float.MaxValue;
        for (int i = 0; i < bones.Count; i++)
        {
            FootIKSolver solver = footIKSolvers[i];
            if (!isGrounded)
            {
                isGrounded = solver.IsGrounded;
            }

            if (solver.IKOffset > lowestOffset)
            {
                lowestOffset = solver.IKOffset;
            }

            if (solver.IKOffset < highestOffset)
            {
                highestOffset = solver.IKOffset;
            }
        }

        lowestOffset = Mathf.Max(lowestOffset, 0f);
        highestOffset = Mathf.Min(highestOffset, 0f);
        pelvis.Process(lowestOffset, highestOffset, isGrounded);
    }
}