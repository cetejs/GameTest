using GameFramework;
using UnityEngine;

public class ThrowingController : MonoBehaviour
{
    [SerializeField]
    private Transform cc;
    [SerializeField]
    private string defaultThrowingId = "ThrowingHub";
    private Transform cam;
    private ParabolaInputs input;
    private Vector3 lastOrigin;
    private Vector3 lastEuler;
    private ThrowingHub hub;
    private bool isThrowing;
    private string throwingId;

    private void Start()
    {
        cam = Camera.main.transform;
        input = GetComponentInParent<ParabolaInputs>();
        UIManager.Instance.ShowWindow("ThrowingWindow");
        throwingId = defaultThrowingId;
    }

    public void Update()
    {
        if (input.IsThrow)
        {
            Vector3 origin = cc.transform.position;
            Vector3 euler = cam.eulerAngles;
            bool isChanged = false;
            isThrowing = true;

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

            if (input.IsCancelThrow)
            {
                CancelThrow();
                input.IsThrow = false;
                input.IsCancelThrow = false;
            }
        }
        else if(isThrowing)
        {
            isThrowing = false;
            StartThrow();
        }
    }

    private void PreThrow(Vector3 origin, Vector3 euler)
    {
        if (!hub)
        {
            hub = AssetManager.Instance.LoadAsset<ThrowingHub>(PathUtils.Combine("Configs/ParabolaTest", throwingId));
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
        ThrowingController con = FindObjectOfType<ThrowingController>();
        if (con)
        {
            con.ChangeThrowing(id);
        }
    }
}