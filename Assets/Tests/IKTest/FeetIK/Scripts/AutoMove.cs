using GameFramework;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private CharacterController cc;
    [SerializeField]
    private Vector3 forward = Vector3.forward;
    [SerializeField]
    private float speed = 1.0f;

    private AnimatorParameter parameter;

    private void Awake()
    {
        parameter = new AnimatorParameter(anim, "Speed");
    }

    private void Update()
    {
        if (cc)
        {
            cc.SimpleMove(forward * speed);
        }

        if (anim && parameter.IsValid)
        {
            anim.SetFloat(parameter, speed);
        }
    }
}
