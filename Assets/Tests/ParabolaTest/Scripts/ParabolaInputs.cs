using GameFramework;
using GameFramework.Samples.SimpleController;
using UnityEngine;

public class ParabolaInputs : SimpleInputs
{
    [SerializeField]
    private bool @throw;
    [SerializeField]
    private bool cancelThrow;

    public bool IsThrow
    {
        get { return @throw; }
        set { @throw = value; }
    }

    public bool IsCancelThrow
    {
        get { return cancelThrow; }
        set { cancelThrow = value; }
    }

    protected override void Update()
    {
        base.Update();
        if (input.GetButtonDown("Throw"))
        {
            @throw = true;
        }

        if (input.GetButtonUp("Throw"))
        {
            
            @throw = false;
        }

        cancelThrow = input.GetButton("CancelThrow");
    }
}