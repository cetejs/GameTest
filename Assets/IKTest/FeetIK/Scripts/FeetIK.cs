using System;
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
    private List<IKBones> bones = new List<IKBones>();

    private FootIKSolver[] footIKSolvers;
    private CCDIKSolver[] ccdIKSolvers;
    private FeetIKPelvis pelvis;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        footIKInfo.root = anim.transform;
        footIKSolvers = new FootIKSolver[bones.Count];
        ccdIKSolvers = new CCDIKSolver[bones.Count];
        pelvis = new FeetIKPelvis(pelvisInfo);
        for (int i = 0; i < bones.Count; i++)
        {
            footIKSolvers[i] = new FootIKSolver(footIKInfo, bones[i].effector);
            ccdIKSolvers[i] = new CCDIKSolver(bones[i]);
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
            ccdIKSolvers[i].SetIKPositionWeight(weight);
            ccdIKSolvers[i].SetIKRotationWeight(weight * footRotationWeight);
            ccdIKSolvers[i].SetIKPosition(footIKSolvers[i].IKPosition);
            ccdIKSolvers[i].SetIKRotation(footIKSolvers[i].IKRotation);
            ccdIKSolvers[i].Process();
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