using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public delegate EventHandler[] playerAttack();
    public event EventHandler OnPlayer1Attack;
    public event EventHandler OnPlayer2Attack;

    public MousePosition mousePosition;

    private PlayerInputActions playerInputActions;

    private void Awake(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player1.Enable();
        playerInputActions.Player1.Attack.performed += Player1_Attack_performed;
        playerInputActions.Player2.Enable();
        playerInputActions.Player2.Attack.performed += Player2_Attack_performed;
    }

    private void Player2_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer2Attack?.Invoke(this, EventArgs.Empty);
    }

    private void Player1_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer1Attack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetKeyboardMovementVectorNormalized(){

        Vector2 inputVector = playerInputActions.Player1.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
    public Vector2 GetJoystickMovementVectorNormalized(){

        Vector2 inputVector = playerInputActions.Player2.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
}
