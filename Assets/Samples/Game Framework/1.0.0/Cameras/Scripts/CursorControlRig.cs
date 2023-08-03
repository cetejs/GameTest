using UnityEngine;
using UnityEngine.EventSystems;

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
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ShowCursor();
            }

            if (lockCursor && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HideCursor();
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
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