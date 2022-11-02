using GameFramework.Generic;
using GameFramework.InputService;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class RebindElement : MonoBehaviour
    {
        [SerializeField]
        private Text buttonNameText;
        [SerializeField]
        private Text keyCodeText;
        [SerializeField]
        private Image joystickIcon;
        [SerializeField]
        private Button rebindButton;
        [SerializeField]
        private Button resetButton;

        private RebindControl control;
        private InputManager input;
        private string buttonName;
        private int boundKey;
        private int inputKey;
        private float inputAxis;
        private bool isCheckRebind;
        private bool isConflict;
        private bool isKeyShow;
        private int frameCount;

        public bool IsConflict
        {
            get { return isConflict; }
        }

        public string ButtonName
        {
            get { return buttonName; }
        }

        public int BoundKey
        {
            get { return boundKey; }
        }

        private void Update()
        {
            if (isCheckRebind && Time.frameCount > frameCount && input.InputKey != 0)
            {
                if (!control.IsRebindAllowed(input.InputName))
                {
                    inputKey = boundKey;
                }
                else
                {
                    inputKey = input.InputKey;
                    inputAxis = input.InputAxis;
                }

                isCheckRebind = false;
                OnListenInput();
            }
        }

        public void Init(RebindControl control, string buttonName)
        {
            this.control = control;
            this.buttonName = buttonName;
            input = Global.GetService<InputManager>();
            buttonNameText.text = input.GetDescription(buttonName);
            SetButtonContent(buttonName);
        }

        public void SetConflict(bool isConflict)
        {
            if (isConflict)
            {
                rebindButton.image.color = Color.red;
            }
            else
            {
                rebindButton.image.color = Color.white;
            }

            this.isConflict = isConflict;
        }

        public void OnRebindButtonClicked()
        {
            if (isCheckRebind)
            {
                return;
            }

            frameCount = Time.frameCount;
            isCheckRebind = true;
            rebindButton.image.color = Color.yellow;
            rebindButton.image.raycastTarget = false;
            resetButton.image.raycastTarget = false;
            keyCodeText.gameObject.SetActive(false);
            joystickIcon.gameObject.SetActive(false);
        }

        public void OnResetButtonClicked()
        {
            isCheckRebind = false;
            inputKey = input.GetActiveInputMapping(buttonName);
            inputAxis = 0.0f;
            OnListenInput();
        }

        private void SetText(string name)
        {
            isKeyShow = true;
            keyCodeText.text = name;
            keyCodeText.gameObject.SetActive(true);
            joystickIcon.gameObject.SetActive(false);
        }

        private void SetIcon(Sprite icon, string name)
        {
            if (icon)
            {
                isKeyShow = false;
                joystickIcon.sprite = icon;
                keyCodeText.gameObject.SetActive(false);
                joystickIcon.gameObject.SetActive(true);
            }
            else
            {
                SetText(name);
            }
        }

        public void SetButtonContent(string name)
        {
            boundKey = input.GetActiveBoundMapping(buttonName);
            string boundName = null;
            switch (input.InputDevice)
            {
                case InputDevice.MouseKeyboard:
                    boundName = ((Keyboard) boundKey).ToString();
                    SetText(boundName);
                    EnableElement(true);
                    break;
                case InputDevice.Mobile:
                    EnableElement(false);
                    break;
                case InputDevice.XboxGamepad:
                    boundName = ((JoystickXbox) boundKey).ToString();
                    SetIcon(control.LoadXboxIcon(boundName), boundName);
                    EnableElement(true);
                    break;
                case InputDevice.Ps4Gamepad:
                    boundName = ((JoystickPs4) boundKey).ToString();
                    SetIcon(control.LoadPs4Icon(boundName), boundName);
                    EnableElement(true);
                    break;
            }
        }

        private void EnableElement(bool isEnable)
        {
            gameObject.SetActive(isEnable);
        }

        private void OnListenInput()
        {
            rebindButton.image.color = Color.white;
            rebindButton.image.raycastTarget = true;
            resetButton.image.raycastTarget = true;
            if (inputKey == boundKey)
            {
                if (isKeyShow)
                {
                    keyCodeText.gameObject.SetActive(true);
                }
                else
                {
                    joystickIcon.gameObject.SetActive(true);
                }

                return;
            }

            int oldBindKey = boundKey;
            boundKey = inputKey;
            input.RebindButton(buttonName, inputKey);
            SetButtonContent(buttonName);
            control.OnButtonRebind(oldBindKey, inputKey);
        }
    }
}