using System.Collections.Generic;

namespace GameFramework.Samples.PersistentData
{
    public class SavingWindow : UIWindow
    {
        public override void Show(object arg)
        {
            base.Show(arg);
            StorageItemData data = (StorageItemData) arg;
            StorageAsyncOperation operation = PersistentManager.Instance.SaveAsync(data.storageName);
            operation.OnCompleted += _ =>
            {
                SetSelectStorageData(data);
                UIManager.Instance.HideWindow(WindowName.Saving);
            };
        }

        private void SetSelectStorageData(StorageItemData data)
        {
            bool isExistSelectStorage = false;
            List<StorageItemData> dataList = PersistentManager.Instance.GetData<List<StorageItemData>>
                (MultiStorageData.SelectStorageName, MultiStorageData.StorageItemsKey);
            if (dataList == null)
            {
                dataList = new List<StorageItemData>();
            }
            else
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].storageName == data.storageName)
                    {
                        dataList[i] = data;
                        isExistSelectStorage = true;
                        break;
                    }
                }
            }

            if (!isExistSelectStorage)
            {
                dataList.Add(data);
            }

            PersistentManager.Instance.SetData(MultiStorageData.SelectStorageName, MultiStorageData.StorageItemsKey, dataList);
            PersistentManager.Instance.SetData(MultiStorageData.SelectStorageName, MultiStorageData.CurrentStorageKey, data.storageName);
            PersistentManager.Instance.Save(MultiStorageData.SelectStorageName);
        }
    }
}