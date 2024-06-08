using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Samples.PersistentData
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private List<UIWindow> windows = new List<UIWindow>();
        private Dictionary<int, UIWindow> windowMap = new Dictionary<int, UIWindow>();

        private void Start()
        {
            for (int i = 0; i < windows.Count; i++)
            {
                windowMap.Add(i, windows[i]);
            }
        }

        public void ShowWindow(WindowName windowName, object arg = null)
        {
            if (windowMap.TryGetValue((int) windowName, out UIWindow window))
            {
                window.Show(arg);
            }
        }

        public void HideWindow(WindowName windowName)
        {
            if (windowMap.TryGetValue((int) windowName, out UIWindow window))
            {
                window.Hide();
            }
        }
    }

    public enum WindowName
    {
        Main,
        StorageSelect,
        Game,
        Loading,
        Saving
    }
}