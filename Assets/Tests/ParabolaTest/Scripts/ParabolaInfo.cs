using System;
using UnityEngine;

[Serializable]
public class ParabolaInfo
{
    public float initialSpeed = 10.0f;
    public float timeInterval = 1.0f;
    public float gravity = 1.0f;
    public int maxPointNum = 50;
    public int maxLength = 500;
    public LayerMask collisionMask;
}