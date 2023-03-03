using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameFramework
{
    public class CursorControlRig : MonoBehaviour
    {
        [SerializeField]
        private bool lockCursor;

#if !MOBILE_INPUT
        private void OnEnable()
        {
            if (lockCursor)
            {
                HideCursor();
            }
        }

        private void OnDisable()
        {
            ShowCursor();
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.isPressed)
            {
                ShowCursor();
            }

            if (lockCursor && Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
            {
                HideCursor();
            }

            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                lockCursor = !lockCursor;
                if (!lockCursor)
                {
                    ShowCursor();
                }
                else
                {
                    HideCursor();
                }
            }
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endif
    }
}