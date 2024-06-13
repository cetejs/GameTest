using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameFramework.Samples.Localization
{
    public class SimpleTest : MonoBehaviour
    {
        [SerializeField]
        private List<string> languageTypes = new List<string> {"CN", "EN", "JP"};
        [SerializeField]
        private List<string> languageDisplays = new List<string>() {"简体中文", "Engine", "日本語"};
        [SerializeField]
        private TMP_Dropdown languageDp;

        private void Start()
        {
            languageDp.AddOptions(languageDisplays);
            languageDp.onValueChanged.AddListener(ChangeLanguage);
        }

        private void ChangeLanguage(int index)
        {
            LocalizationManager.Instance.ChangeLanguage(languageTypes[index]);
        }
    }
}