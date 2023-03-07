using GameFramework;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnThrow(InputValue value)
    {
        @throw = value.isPressed;
    }

    public void OnCancelThrow(InputValue value)
    {
        cancelThrow = value.isPressed;
    }
}