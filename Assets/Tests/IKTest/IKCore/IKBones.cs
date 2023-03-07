using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IKBones
{
    public Transform effector;
    public Transform baseBone;
    private List<Transform> bones = new List<Transform>();

    public int Count
    {
        get { return bones.Count; }
    }

    public Transform this[int i]
    {
        get { return bones[i]; }
    }

    public void Update()
    {
        bones.Clear();
        Transform current = effector;
        while (current != null && current != baseBone.parent)
        {
            bones.Add(current);
            current = current.parent;
        }

        if (current == null)
        {
            bones.Clear();
        }
    }
}