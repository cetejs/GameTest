using UnityEditor;
using UnityEngine;

namespace PhysicsTest
{
    [CustomEditor(typeof(Collider2D), true)]
    public class Collider2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            Collider2D col = target as Collider2D;
            Vector3 offset = EditorGUILayout.Vector2Field("Offset", col.bounds.center);
            Vector3 size = EditorGUILayout.Vector2Field("Size", col.bounds.extent);
            size = Vector3Utils.Max(size, Vector3.zero);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(col, "Update Collider2D");
                col.bounds.center = offset;
                col.bounds.extent = size;
                SceneView.RepaintAll();
            }
        }
    }
}