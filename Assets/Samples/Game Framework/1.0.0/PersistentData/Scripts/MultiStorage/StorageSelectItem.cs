using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Samples.PersistentData
{
    public class StorageSelectItem : PoolObject
    {
        [SerializeField]
        private Button loadBtn;
        [SerializeField]
        private Button deleteBtn;
        [SerializeField]
        private Text titleText;

        private StorageSelectWindow window;
        private StorageItemData data;

        public string StorageName
        {
            get { return data.storageName; }
        }

        public void SetData(StorageSelectWindow window, StorageItemData data)
        {
            this.window = window;
            this.data = data;
            titleText.text = data.title;
        }

        private void Start()
        {
            loadBtn.onClick.AddListener(LoadStorage);
            deleteBtn.onClick.AddListener(DeleteStorage);
        }

        private void LoadStorage()
        {
            window.LoadStorage(this);
        }

        private void DeleteStorage()
        {
            window.DeleteStorage(this);
        }
    }
}