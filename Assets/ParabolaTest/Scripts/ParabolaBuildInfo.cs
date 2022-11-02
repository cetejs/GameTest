using System.Collections.Generic;
using UnityEngine;

public struct ParabolaBuildInfo
{
   public ParabolaInfo fixedInfo;
   public Vector3 origin;
   public Vector3 forward;
   public List<Vector3> points;
   public List<Vector3> dirs;
}