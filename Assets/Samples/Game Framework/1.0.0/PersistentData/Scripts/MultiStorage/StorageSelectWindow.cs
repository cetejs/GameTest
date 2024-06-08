using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Samples.PersistentData
{
    public class StorageSelectWindow : UIWindow
    {
        [SerializeField]
        private StorageSelectItem itemPrefab;
        [SerializeField]
        private Transform itemParent;
        [SerializeField]
        private Text emptyText;
        [SerializeField]
        private Button returnMenuBtn;
        private ObjectPool<StorageSelectItem> itemPool;
        private List<StorageSelectItem> items = new List<StorageSelectItem>();
        private List<StorageItemData> dataList;

        private void Awake()
        {
            itemPool = new ObjectPool<StorageSelectItem>(transform);
            itemPool.Init(itemPrefab);
        }

        private void Start()
        {
            returnMenuBtn.onClick.AddListener(ReturnMenu);
        }

        public override void Show(object arg)
        {
            base.Show(arg);
            for (int i = 0; i < items.Count; i++)
            {
                itemPool.Release(items[i]);
            }

            items.Clear();
            dataList = PersistentManager.Instance.GetData<List<StorageItemData>>
                (MultiStorageData.SelectStorageName, MultiStorageData.StorageItemsKey);
            if (dataList != null)
            {
                foreach (StorageItemData data in dataList)
                {
                    StorageSelectItem selectItem = itemPool.Get(itemParent);
                    selectItem.SetData(this, data);
                    items.Add(selectItem);
                }
            }

            emptyText.gameObject.SetActive(items.Count == 0);
        }

        public void LoadStorage(StorageSelectItem item)
        {
            UIManager.Instance.HideWindow(WindowName.StorageSelect);
            UIManager.Instance.ShowWindow(WindowName.Loading, item.StorageName);
            PersistentManager.Instance.SetData(MultiStorageData.SelectStorageName, MultiStorageData.CurrentStorageKey, item.StorageName);
            PersistentManager.Instance.Save(MultiStorageData.SelectStorageName);
        }

        public void DeleteStorage(StorageSelectItem item)
        {
            items.Remove(item);
            itemPool.Release(item);
            dataList.RemoveAll(data => data.storageName == item.StorageName);
            PersistentManager.Instance.Delete(item.StorageName);
            PersistentManager.Instance.SetData(MultiStorageData.SelectStorageName, MultiStorageData.StorageItemsKey, dataList);
            PersistentManager.Instance.Save(MultiStorageData.SelectStorageName);
            if (items.Count == 0)
            {
                PersistentManager.Instance.Delete(MultiStorageData.SelectStorageName);
            }
            else
            {
                string currentStorage = PersistentManager.Instance.GetData<string>(MultiStorageData.SelectStorageName, MultiStorageData.CurrentStorageKey);
                if (currentStorage == item.StorageName)
                {
                    PersistentManager.Instance.DeleteKey(MultiStorageData.SelectStorageName, MultiStorageData.CurrentStorageKey);
                    PersistentManager.Instance.Save(MultiStorageData.SelectStorageName);
                }
            }

            emptyText.gameObject.SetActive(items.Count == 0);
        }

        private void ReturnMenu()
        {
            UIManager.Instance.HideWindow(WindowName.StorageSelect);
            UIManager.Instance.ShowWindow(WindowName.Main);
        }
    }

    [Serializable]
    public struct StorageItemData
    {
        public string storageName;
        public string title;
    }
}