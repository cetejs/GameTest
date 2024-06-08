using UnityEngine;

namespace GameFramework.Samples.Simple
{
    public class SimpleTest : MonoBehaviour
    {
        [SerializeField]
        private string storageName;

        private void Reset()
        {
            storageName = PersistentSetting.Instance.DefaultStorageName;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load", GUILayout.Width(100)))
            {
                PersistentManager.Instance.Load(storageName);
            }

            if (GUILayout.Button("Save", GUILayout.Width(100)))
            {
                PersistentManager.Instance.Save(storageName);
            }

            GUILayout.EndHorizontal();
        }
    }
}