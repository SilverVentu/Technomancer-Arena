using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class GameInput : MonoBehaviour
{
    [SerializeField] private Vector2 refVelocity;
    [SerializeField] private float aimSpeed;
    public delegate EventHandler[] playerAttack();
    public event EventHandler OnPlayer1Attack;
    public event EventHandler OnPlayer2Attack;
    public event EventHandler<OnPlayer1DashEventArgs> OnPlayer1Dash;
    public event EventHandler<OnPlayer2DashEventArgs> OnPlayer2Dash;

    public MousePosition mousePosition;

    private PlayerInputActions playerInputActions;
    private Vector2 smoothVector;
    public class OnPlayer1DashEventArgs : EventArgs
    {
        public int player;
    }
    public class OnPlayer2DashEventArgs : EventArgs
    {
        public int player;
    }

    private void Awake(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player1.Enable();
        playerInputActions.Player2.Enable();
        playerInputActions.Player1.Attack.performed += Player1_Attack_performed;
        playerInputActions.Player2.Attack.performed += Player2_Attack_performed;
        playerInputActions.Player1.Dash.performed += Player1_Dash_performed;
        playerInputActions.Player2.Dash.performed += Playe2_Dash_performed1;
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
    public Vector2 GetJoystickAimDirectionVectorNormalized(){

        Vector2 inputVector = playerInputActions.Player2.AimDirection.ReadValue<Vector2>();
        smoothVector = Vector2.SmoothDamp(smoothVector, inputVector, ref refVelocity, aimSpeed);
        //inputVector = inputVector.normalized;
        return smoothVector;
    }

    private void Player1_Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer1Dash?.Invoke(this, new OnPlayer1DashEventArgs { player = 0 });
    }

    private void Playe2_Dash_performed1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer2Dash?.Invoke(this, new OnPlayer2DashEventArgs { player = 1 });
    }

    private void Player2_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer2Attack?.Invoke(this, EventArgs.Empty);
    }

    private void Player1_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayer1Attack?.Invoke(this, EventArgs.Empty);
        Debug.Log("pew");
    }
}
