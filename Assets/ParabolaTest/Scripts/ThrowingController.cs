using GameFramework.DevConsoleService;
using GameFramework.Generic;
using GameFramework.InputService;
using GameFramework.UIService;
using GameFramework.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ThrowingController : MonoBehaviour
{
    [SerializeField]
    private Transform cc;
    [SerializeField]
    private string defaultThrowingId = "ThrowingHub";
    private Transform cam;
    private InputManager input;
    private Vector3 lastOrigin;
    private Vector3 lastEuler;
    private ThrowingHub hub;
    private bool isThrowing;
    private string throwingId;

    private void Start()
    {
        cam = Camera.main.transform;
        input = Global.GetService<InputManager>();
        Global.GetService<UIManager>().ShowWindow("ThrowingWindow");
        throwingId = defaultThrowingId;
    }

    public void Update()
    {
        if (input.GetButtonDown("Throwing"))
        {
            isThrowing = true;
        }

        if (isThrowing)
        {
            Vector3 origin = cc.transform.position;
            Vector3 euler = cam.eulerAngles;
            bool isChanged = false;

            if (!VectorUtils.Approximately(origin, lastOrigin))
            {
                lastOrigin = origin;
                isChanged = true;
            }

            if (!VectorUtils.Approximately(euler, lastEuler))
            {
                lastEuler = origin;
                isChanged = true;
            }

            if (isChanged)
            {
                PreThrow(origin, euler);
            }

            if (input.GetButtonUp("Throwing") || input.GetButtonDown("StartThrowing"))
            {
                StartThrow();
                isThrowing = false;
            }

            if (input.GetButtonDown("CancelThrowing"))
            {
                CancelThrow();
                isThrowing = false;
            }
        }
    }

    private async void PreThrow(Vector3 origin, Vector3 euler)
    {
        if (!hub)
        {
            hub = await Addressables.LoadAssetAsync<ThrowingHub>(string.Concat("Configs/", throwingId, ".asset")).Task;
        }

        hub.PreThrow(origin, euler);
    }

    private void StartThrow()
    {
        if (hub)
        {
            hub.StartThrow();
            hub = null;
        }
    }

    private void CancelThrow()
    {
        if (hub)
        {
            hub.CancelThrow();
            hub = null;
        }
    }

    public void ChangeThrowing(string id)
    {
        throwingId = id;
    }

    [DevCmd("Throwing", -1, "ThrowingHub", "ThrowingHubA", "ThrowingHubB")]
    public static void TestChangeThrowing(string id)
    {
        ThrowingController con =  FindObjectOfType<ThrowingController>();
        if (con)
        {
            con.ChangeThrowing(id);
        }
    }
}