using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace GameFramework.Samples.PersistentData
{
    public class GameWindow : UIWindow
    {
        [SerializeField]
        private InputField nickNameIpt;
        [SerializeField]
        private Dropdown ageDp;
        [SerializeField]
        private InputField ageIpt;
        [SerializeField]
        private PlayerInfoItem itemPrefab;
        [SerializeField]
        private Transform itemParent;
        [SerializeField]
        private Button expandBtn;
        [SerializeField]
        private Button saveBtn;
        [SerializeField]
        private Button returnMenuBtn;
        private List<PlayerInfoItem> items = new List<PlayerInfoItem>();
        private PlayerInfo info;
        private List<PlayerInfo> infos;
        private string storageName;

        private void Awake()
        {
            for (int i = 0; i <= (int) PlayerProperty.Luck; i++)
            {
                items.Add(Instantiate(itemPrefab, itemParent));
            }
        }

        private void Start()
        {
            nickNameIpt.onEndEdit.AddListener(ChangeName);
            ageDp.onValueChanged.AddListener(ChangeSxe);
            ageIpt.onEndEdit.AddListener(ChangeAge);
            expandBtn.onClick.AddListener(Expand);
            saveBtn.onClick.AddListener(Save);
            returnMenuBtn.onClick.AddListener(ReturnMenu);
        }

        public override void Show(object arg)
        {
            base.Show(arg);
            storageName = (string) arg;
            info = PersistentManager.Instance.GetData<PlayerInfo>(storageName, MultiStorageData.PlayerInfoKey);
            if (info == null)
            {
                info = CreateDefaultPlayerInfo();
            }

            nickNameIpt.text = info.nickName;
            ageDp.SetValueWithoutNotify((int) info.sxe);
            ageIpt.text = info.age.ToString();

            for (int i = 0; i <= (int) PlayerProperty.Luck; i++)
            {
                items[i].SetData((PlayerProperty) i, GetPropertyName((PlayerProperty) i), info.properties[i]);
            }
        }

        public void ChangeProperty(PlayerProperty property, int num)
        {
            info.properties[(int) property] = num;
        }

        private PlayerInfo CreateDefaultPlayerInfo()
        {
            PlayerInfo info = new PlayerInfo()
            {
                nickName = "DefaultNickName",
                sxe = PlayerSxe.Male,
                age = 18,
                properties = new Dictionary<int, int>()
            };

            for (int i = 0; i <= (int) PlayerProperty.Luck; i++)
            {
                info.properties.Add(i, 10);
            }

            return info;
        }

        private void ChangeName(string text)
        {
            info.nickName = text;
        }

        private void ChangeSxe(int index)
        {
            info.sxe = (PlayerSxe) index;
        }

        private void ChangeAge(string text)
        {
            info.age = int.Parse(text);
        }

        private void Expand()
        {
            if (infos == null)
            {
                infos = new List<PlayerInfo>();
            }

            for (int i = 0; i < 1000000; i++)
            {
                infos.Add(CreateDefaultPlayerInfo());
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            PersistentManager.Instance.SetData(storageName, MultiStorageData.ExpandInfoKey, infos);
            stopwatch.Stop();
            Debug.Log($"[{PersistentSetting.Instance.StorageMode}] => Expand = {infos.Count} Milliseconds = {stopwatch.ElapsedMilliseconds}");
        }

        private void Save()
        {
            PersistentManager.Instance.SetData(storageName, MultiStorageData.PlayerInfoKey, info);
            UIManager.Instance.ShowWindow(WindowName.Saving, new StorageItemData()
            {
                storageName = storageName,
                title = info.nickName
            });
        }

        private void ReturnMenu()
        {
            if (PersistentManager.Instance.GetStorage(storageName).State == PersistentState.Saving)
            {
                return;
            }

            PersistentManager.Instance.Unload(storageName);
            UIManager.Instance.HideWindow(WindowName.Game);
            UIManager.Instance.ShowWindow(WindowName.Main);
        }

        private string GetPropertyName(PlayerProperty property)
        {
            switch (property)
            {
                case PlayerProperty.Vitality:
                    return "体力";
                case PlayerProperty.Strength:
                    return "力量";
                case PlayerProperty.Agility:
                    return "敏捷";
                case PlayerProperty.Endurance:
                    return "耐力";
                case PlayerProperty.Intelligence:
                    return "智力";
                case PlayerProperty.Luck:
                    return "运气";
            }

            return null;
        }
    }

    [Serializable]
    public class PlayerInfo
    {
        public string nickName;
        public PlayerSxe sxe;
        public int age;
        public Dictionary<int, int> properties;
    }

    [Serializable]
    public enum PlayerSxe
    {
        Male,
        Female
    }

    [Serializable]
    public enum PlayerProperty
    {
        Vitality,
        Strength,
        Agility,
        Endurance,
        Intelligence,
        Luck
    }
}