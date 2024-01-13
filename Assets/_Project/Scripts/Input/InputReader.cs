using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, CustomInput.IPlayerActions, CustomInput.IVendingActions
{

    private CustomInput customInput;

    private void OnEnable()
    {
        if (customInput == null) {
            customInput = new CustomInput();

            customInput.Player.SetCallbacks(this);
            customInput.Vending.SetCallbacks(this);

            EnablePlayer();
            EnableVending();
        }
    }

    public event Action<float> MoveEvent;
    public event Action DashEvent;
    public event Action<float> GlideEvent;
    public event Action JumpEvent;
    public event Action OpenVendingEvent;
    public event Action PoundEvent;

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed) {
            DashEvent?.Invoke();
        }
    }

    public void OnGlide(InputAction.CallbackContext context)
    {
        GlideEvent?.Invoke(context.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) {
            JumpEvent?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<float>());
    }

    public void OnOpenVendingMachine(InputAction.CallbackContext context)
    {
        if (context.performed) {
            OpenVendingEvent?.Invoke();
        }
    }

    public void OnPound(InputAction.CallbackContext context)
    {
        if (context.performed) {
            PoundEvent?.Invoke();
        }
    }

    public void DisablePlayer() {
        customInput.Player.Disable();
    }

    public void EnablePlayer() {
        customInput.Player.Enable();
    }

    public void EnableVending() {
        customInput.Vending.Enable();
    }

    public void DisableVending() {
        customInput.Vending.Disable();
    }
}
