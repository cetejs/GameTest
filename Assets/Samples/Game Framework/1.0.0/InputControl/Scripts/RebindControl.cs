using System;
using System.Collections.Generic;
using GameFramework.Generic;
using GameFramework.InputService;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework
{
    public class RebindControl : MonoBehaviour
    {
        [SerializeField]
        private Transform rebindElementParent;
        private RebindElement rebindElementPrefab;
        private Dictionary<string, RebindElement> rebindElements = new Dictionary<string, RebindElement>();
        private List<RebindElement> conflictElements = new List<RebindElement>();

        private readonly Dictionary<string, string> xboxIconNames = new Dictionary<string, string>()
        {
            {"D-Pad", "XboxOne_Dpad"},
            {"D-Pad-Up", "XboxOne_Dpad_Up"},
            {"D-Pad-Down", "XboxOne_Dpad_Down"},
            {"D-Pad-Left", "XboxOne_Dpad_Left"},
            {"D-Pad-Right", "XboxOne_Dpad_Right"},
            {"A", "XboxOne_A"},
            {"B", "XboxOne_B"},
            {"X", "XboxOne_X"},
            {"Y", "XboxOne_Y"},
            {"LB", "XboxOne_LB"},
            {"RB", "XboxOne_RB"},
            {"Back", "XboxOne_Menu"},
            {"Start", "XboxOne_Windows"},
            {"LT", "XboxOne_LT"},
            {"RT", "XboxOne_RT"},
            {"LeftStickClick", "XboxOne_Left_Stick"},
            {"RightStickClick", "XboxOne_Right_Stick"}
        };

        private readonly Dictionary<string, string> ps4IconNames = new Dictionary<string, string>()
        {
            {"D-Pad", "PS4_Dpad"},
            {"D-Pad-Up", "PS4_Dpad_Up"},
            {"D-Pad-Down", "PS4_Dpad_Down"},
            {"D-Pad-Left", "PS4_Dpad_Left"},
            {"D-Pad-Right", "PS4_Dpad_Right"},
            {"Cross", "PS4_Cross"},
            {"Circle", "PS4_Circle"},
            {"Square", "PS4_Square"},
            {"Triangle", "PS4_Triangle"},
            {"L1", "PS4_L1"},
            {"R1", "PS4_R1"},
            {"Share", "PS4_Share"},
            {"Options", "PS4_Options"},
            {"L2", "PS4_L2"},
            {"R2", "PS4_R2"},
            {"LeftStickClick", "PS4_Left_Stick"},
            {"RightStickClick", "PS4_Right_Stick"}
        };
        
        private readonly List<string> buttonNames = new List<string>
        {
            "Button0",
            "Button1",
            "Button2",
            "Button3",
            "Button4",
            "Button5",
            "Button6",
            "Button7",
            "Button8",
            "Button9",
            "LeftStickClick",
            "RightStickClick",
        };

        private Dictionary<string, Sprite> xboxIcons = new Dictionary<string, Sprite>(20);
        private Dictionary<string, Sprite> ps4Icons = new Dictionary<string, Sprite>(20);

        private InputManager input;

        private void Start()
        {
            input = Global.GetService<InputManager>();
            foreach (string name in buttonNames)
            {
                AddRebindElement(name);
            }
            
            foreach (string name in buttonNames)
            {
                CheckConflict(input.GetActiveBoundMapping(name));
            }

            input.OnDeviceChanged += device =>
            {
                foreach (KeyValuePair<string, RebindElement> kvPair in rebindElements)
                {
                    string name = kvPair.Key;
                    RebindElement element = kvPair.Value;
                    element.SetButtonContent(name);
                }
            };
        }

        private void Update()
        {
            input.SelectDefaultGo();
        }

        public Sprite LoadXboxIcon(string name)
        {
            if (xboxIcons.TryGetValue(name, out Sprite icon))
            {
                return icon;
            }

            if (xboxIconNames.TryGetValue(name, out string iconName))
            {
                icon = Resources.Load<Sprite>(string.Concat("Icons/", iconName));
                if (icon)
                {
                    xboxIcons.Add(name, icon);
                }
            }

            if (!icon)
            {
                Debug.LogError($"Dont exist xbox icon {name}");
            }

            return icon;
        }

        public Sprite LoadPs4Icon(string name)
        {
            if (ps4Icons.TryGetValue(name, out Sprite icon))
            {
                return icon;
            }

            if (ps4IconNames.TryGetValue(name, out string iconName))
            {
                icon = Resources.Load<Sprite>(string.Concat("Icons/", iconName));
                if (icon)
                {
                    ps4Icons.Add(name, icon);
                }
            }

            return icon;
        }

        public bool IsRebindAllowed(string name)
        {
            switch (input.InputDevice)
            {
                case InputDevice.XboxGamepad:
                    return xboxIconNames.ContainsKey(name);
                case InputDevice.Ps4Gamepad:
                    return ps4IconNames.ContainsKey(name);
            }

            return true;
        }

        public void AddRebindElement(string name)
        {
            if (rebindElements.ContainsKey(name))
            {
                return;
            }

            if (!rebindElementPrefab)
            {
                rebindElementPrefab = Resources.Load<GameObject>("RebindElement").GetComponent<RebindElement>();
            }

            RebindElement element = Instantiate(rebindElementPrefab);
            element.Init(this, name);
            element.transform.SetParent(rebindElementParent);
            rebindElements.Add(name, element);
        }

        public void OnButtonRebind(int oldKey, int newKey)
        {
            CheckConflict(oldKey);
            CheckConflict(newKey);
        }
        
        public string AxisToButton(string axis, float value)
        {
            switch (axis)
            {
                case "D-Pad Horizontal":
                    return value > 0 ? "D-Pad-Right" : "D-Pad-Left";
                case "D-Pad Vertical":
                    return value > 0 ? "D-Pad-Up" : "D-Pad-Down";
            }

            return axis;
        }

        private void CheckConflict(int key)
        {
            foreach (RebindElement element in rebindElements.Values)
            {
                if (element.BoundKey == key)
                {
                    conflictElements.Add(element);
                }
            }

            bool isConflict = conflictElements.Count > 1;
            foreach (RebindElement element in conflictElements)
            {
                element.SetConflict(isConflict);
            }

            conflictElements.Clear();
        }
    }
}