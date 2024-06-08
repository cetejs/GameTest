using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Samples.PersistentData
{
    public class MainWindow : UIWindow
    {
        [SerializeField]
        private Button startNewGameBtn;
        [SerializeField]
        private Button continueGameBtn;
        [SerializeField]
        private Button loadGameBtn;
        [SerializeField]
        private Button openSettingBtn;
        [SerializeField]
        private Button quitGameBtn;

        public override void Show(object arg)
        {
            base.Show(arg);
            Refresh();
        }

        private void Start()
        {
            startNewGameBtn.onClick.AddListener(StartNewGame);
            continueGameBtn.onClick.AddListener(ContinueGame);
            loadGameBtn.onClick.AddListener(LoadGame);
            openSettingBtn.onClick.AddListener(OpenSetting);
            quitGameBtn.onClick.AddListener(QuitGame);
            Refresh();
        }

        private void StartNewGame()
        {
            string currentStorageName = StringUtils.Concat(MultiStorageData.GameStorageName, Guid.NewGuid().ToString());
            UIManager.Instance.HideWindow(WindowName.Main);
            UIManager.Instance.ShowWindow(WindowName.Game, currentStorageName);
        }

        private void ContinueGame()
        {
            string currentStorageName = PersistentManager.Instance.GetData<string>(MultiStorageData.SelectStorageName, MultiStorageData.CurrentStorageKey);
            UIManager.Instance.HideWindow(WindowName.Main);
            UIManager.Instance.ShowWindow(WindowName.Loading, currentStorageName);
        }

        private void LoadGame()
        {
            UIManager.Instance.HideWindow(WindowName.Main);
            UIManager.Instance.ShowWindow(WindowName.StorageSelect);
        }

        private void OpenSetting()
        {
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void Refresh()
        {
            string currentStorageName = PersistentManager.Instance.GetData<string>(MultiStorageData.SelectStorageName, MultiStorageData.CurrentStorageKey);
            continueGameBtn.gameObject.SetActiveSafe(currentStorageName != null);
        }
    }
}