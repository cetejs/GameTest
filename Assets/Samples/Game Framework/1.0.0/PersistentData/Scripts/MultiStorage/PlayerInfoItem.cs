using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Samples.PersistentData
{
    public class PlayerInfoItem : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text numText;
        [SerializeField]
        private Button addBtn;
        [SerializeField]
        private Button subBtn;
        private GameWindow window;
        private PlayerProperty property;
        private int num;

        public void SetData(PlayerProperty property, string name, int num)
        {
            this.property = property;
            this.num = num;
            nameText.text = name;
            numText.text = num.ToString();
        }

        private void Awake()
        {
            window = GetComponentInParent<GameWindow>();
        }

        private void Start()
        {
            addBtn.onClick.AddListener(AddProperty);
            subBtn.onClick.AddListener(SubProperty);
        }

        private void AddProperty()
        {
            num = Mathf.Clamp(++num, 0, 99);
            numText.text = num.ToString();
            window.ChangeProperty(property, num);
        }

        private void SubProperty()
        {
            num = Mathf.Clamp(--num, 0, 99);
            numText.text = num.ToString();
            window.ChangeProperty(property, num);
        }
    }
}