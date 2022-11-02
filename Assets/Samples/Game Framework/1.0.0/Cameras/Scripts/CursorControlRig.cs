using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework
{
    public class CursorControlRig : MonoBehaviour
    {
        [SerializeField]
        private bool isLockCursor;

#if !MOBILE_INPUT
        private void OnEnable()
        {
            if (isLockCursor)
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowCursor();
            }

            if (isLockCursor && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HideCursor();
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                isLockCursor = !isLockCursor;
                if (!isLockCursor)
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