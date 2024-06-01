using UnityEngine;

namespace GameFramework
{
    public class DataTableTest : MonoBehaviour
    {
        private void OnEnable()
        {
            DataTableManager.Instance.ReloadTable<Test1>();
            
            for (int i = 1; i < 11; i++)
            {
                Debug.Log("[Test1] " + DataTableManager.Instance.GetTable<Test1>(i.ToString()));
            }
            
            for (int i = 1; i < 11; i++)
            {
                Debug.Log("[Test2] " + DataTableManager.Instance.GetTable<Test2>(i.ToString()));
            }
        }
    }
}