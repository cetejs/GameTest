using System;
using UnityEngine;

[Serializable]
public class ThrowingMoveInfo
{
    public float moveSpeed = 10.0f;
    public bool isIgnoreCollider;
    public LayerMask collisionMask;
}