using GameFramework.Generic;
using GameFramework.InputService;
using UnityEngine;

namespace GameFramework
{
    public class InputGUITest : MonoBehaviour
    {
        private KeyCode keyCode;
        private string inputName;
        private float inputAxis;

        private void OnGUI()
        {
            InputManager input = Global.GetService<InputManager>();
            GUILayout.TextArea($"InputDevice : {input.InputDevice}");
            GUILayout.TextArea($"Horizontal : {input.GetAxis("Horizontal")}");
            GUILayout.TextArea($"Vertical : {input.GetAxis("Vertical")}");
            GUILayout.TextArea($"Mouse X : {input.GetAxis("Mouse X")}");
            GUILayout.TextArea($"Mouse Y : {input.GetAxis("Mouse Y")}");
            GUILayout.TextArea($"HorizontalArrow : {input.GetAxis("HorizontalArrow")}");
            GUILayout.TextArea($"VerticalArrow : {input.GetAxis("VerticalArrow")}");
            GUILayout.TextArea($"Button0 : {input.GetButton("Button0")}");
            GUILayout.TextArea($"Button1 : {input.GetButton("Button1")}");
            GUILayout.TextArea($"Button2 : {input.GetButton("Button2")}");
            GUILayout.TextArea($"Button3 : {input.GetButton("Button3")}");
            GUILayout.TextArea($"Button4 : {input.GetButton("Button4")}");
            GUILayout.TextArea($"Button5 : {input.GetButton("Button5")}");
            GUILayout.TextArea($"Button6 : {input.GetButton("Button6")}");
            GUILayout.TextArea($"Button7 : {input.GetButton("Button7")}");
            GUILayout.TextArea($"Button8 : {input.GetButton("Button8")}");
            GUILayout.TextArea($"Button9 : {input.GetButton("Button9")}");
            GUILayout.TextArea($"Button10 : {input.GetButton("Button10")}");
            GUILayout.TextArea($"LeftStickClick : {input.GetButton("LeftStickClick")}");
            GUILayout.TextArea($"RightStickClick : {input.GetButton("RightStickClick")}");

            GUILayout.TextArea($"x axis : {Input.GetAxis("x axis")}");
            GUILayout.TextArea($"y axis : {Input.GetAxis("y axis")}");
            GUILayout.TextArea($"3rd axis : {Input.GetAxis("3rd axis")}");
            GUILayout.TextArea($"4th axis : {Input.GetAxis("4th axis")}");
            GUILayout.TextArea($"5th axis : {Input.GetAxis("5th axis")}");
            GUILayout.TextArea($"6th axis : {Input.GetAxis("6th axis")}");
            GUILayout.TextArea($"8th axis : {Input.GetAxis("8th axis")}");
            GUILayout.TextArea($"9th axis : {Input.GetAxis("9th axis")}");
            GUILayout.TextArea($"10th axis : {Input.GetAxis("10th axis")}");

            if (Event.current.isKey && Event.current.keyCode != KeyCode.None)
            {
                keyCode = Event.current.keyCode;
            }

            if (!string.IsNullOrEmpty(input.InputName))
            {
                inputName = input.InputName;
                inputAxis = input.InputAxis;
            }

            GUILayout.TextArea($"KeyCode : {keyCode}");
            GUILayout.TextArea($"InputName : {inputName}");
            GUILayout.TextArea($"InputAxis : {inputAxis}");
        }
    }
}