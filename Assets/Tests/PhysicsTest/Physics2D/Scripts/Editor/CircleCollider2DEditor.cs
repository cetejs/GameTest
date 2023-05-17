using UnityEditor;
using UnityEngine;

namespace PhysicsTest
{
    [CustomEditor(typeof(CircleCollider2D), true)]
    public class CircleCollider2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            CircleCollider2D col = target as CircleCollider2D;
            Vector3 offset = EditorGUILayout.Vector2Field("Offset", col.bounds.center);
            float radius = EditorGUILayout.FloatField("Radius", col.Radius);
            radius = Mathf.Max(radius, 0);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(col, "Update Collider2D");
                col.bounds.center = offset;
                col.Radius = radius;
                SceneView.RepaintAll();
            }
        }
    }
}