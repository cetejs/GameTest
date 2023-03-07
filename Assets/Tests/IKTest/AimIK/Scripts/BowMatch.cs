using GameFramework.Generic;
using UnityEngine;

public class BowMatch : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform bowstringBone;
    [SerializeField]
    private Transform pullHand;
    [SerializeField]
    private string bowstringMatchCurve = "BowstringMatch";
    [SerializeField]
    private float restoreSpeed = 1.0f;

    private AnimatorParameter matchParameter;
    private Vector3 bowstringOriginPos;

    private void Awake()
    {
        if (!anim)
        {
            return;
        }

        matchParameter = new AnimatorParameter(anim, bowstringMatchCurve);
        bowstringOriginPos = bowstringBone.localPosition;
    }

    private void Update()
    {
        if (!anim || !bowstringBone || !pullHand)
        {
            return;
        }

        bool isMatch = false;
        if (matchParameter.isValid)
        {
            isMatch = anim.GetFloat(matchParameter) >= 1.0f;
        }

        if (isMatch)
        {
            bowstringBone.position = pullHand.position;
        }
        else
        {
            bowstringBone.localPosition = Vector3.Lerp(bowstringBone.localPosition, bowstringOriginPos, restoreSpeed * Time.deltaTime);
        }
    }
}