using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private Vector2 refVelocity;
    [SerializeField] private float aimSpeed;
    public event EventHandler OnPlayer1Attack;
    public event EventHandler OnPlayerDash;

    public MousePosition mousePosition;

    private PlayerInputActions playerInputActions;


    private void Awake(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player1.Enable();
        playerInputActions.Player1.Attack.performed += Player1_Attack_performed;
        playerInputActions.Player1.Dash.performed += Player1_Dash_performed;
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



    private void Player1_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer1Attack?.Invoke(this, EventArgs.Empty);
    }
}
