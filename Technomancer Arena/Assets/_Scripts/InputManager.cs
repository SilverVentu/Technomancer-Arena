using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Vector2 refVelocity;
    [SerializeField] private float aimSpeed;
    public event EventHandler OnPlayerShoot, OnPlayerDash, OnTakeAim, OnLowerAim, OnPlayerStopShooting;

    public MousePosition mousePosition;

    private PlayerInputActions playerInputActions;


    private void Awake(){
        mousePosition = GetComponent<MousePosition>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player1.Enable();
        playerInputActions.Player1.PlayerShoot.performed += PlayerShoot_performed;
        playerInputActions.Player1.PlayerShoot.canceled += PlayerShoot_canceled;
        playerInputActions.Player1.PlayerTakeAim.performed += PlayerTakeAim_performed;
        playerInputActions.Player1.PlayerTakeAim.canceled += PlayerTakeAim_canceled;
        playerInputActions.Player1.Dash.performed += Player1_Dash_performed;
    }

    private void PlayerShoot_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerStopShooting?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerTakeAim_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnLowerAim?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerTakeAim_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnTakeAim?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetKeyboardMovementVectorNormalized(){

        Vector2 inputVector = playerInputActions.Player1.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Player1_Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }



    private void PlayerShoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerShoot?.Invoke(this, EventArgs.Empty);
    }
}
