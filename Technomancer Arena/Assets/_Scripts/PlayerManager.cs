using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour, IHasHealth
{
    [SerializeField] private int player;
    [SerializeField] private float health;
    [SerializeField] private float runSpeed, aimingSpeed, speedChangeSpeed, dashSpeed, dashLenght, dashCooldown;
    [SerializeField] private LayerMask Undashable, collideWith;
    [SerializeField] private GunDroneOptions equipedGunDrone;
    [SerializeField] private Transform gunDroneAnchorPoint;
    [SerializeField] private state currentState, previosState;

    private InputManager inputManager;
    private bool isWalking;
    private Vector3 lastDirectionDir, moveDir, dashOrigin, lastPosition;
    private float moveDistance, dashCooldownTimer = 0, targetMoveSpeed, actualMoveSpeed;
    private delegate void AvailableActions();

    private AvailableActions availableActions;

    private enum state
    {
        NEUTRAL,
        DASHING,
        COOLDOWNING,
        AIMING,
    }
    public enum GunDroneOptions
    {
        SPADA,
        SCOPETA,
    }


    private void Start()
    {
        inputManager = DATA.Instance.inputManager;

        SetState(state.NEUTRAL);

        inputManager.OnPlayerDash += GameInput_OnPlayerDash;
        inputManager.OnTakeAim += InputManager_OnTakeAim;
        inputManager.OnLowerAim += InputManager_OnLowerAim;
    }

    private void InputManager_OnLowerAim(object sender, EventArgs e)
    {
        SetState(state.NEUTRAL);
    }

    private void InputManager_OnTakeAim(object sender, EventArgs e)
    {
        if(currentState == state.NEUTRAL)
        {
            SetState(state.AIMING);
        }
    }

    private void GameInput_OnPlayerDash(object sender, EventArgs e)
    {
        if (CanDash())
        {
            dashOrigin = transform.position;
            SetState(state.DASHING);
        }
    }



    private void Update()
    {
        availableActions();
        actualMoveSpeed = Mathf.Lerp(actualMoveSpeed, targetMoveSpeed, speedChangeSpeed * Time.deltaTime);

    }

    private void SetState(state state)
    {
        previosState = currentState;
        currentState = state;
        switch (state)
        {
            case state.NEUTRAL:
                StopCoroutine(DecreaseSpeed());
                targetMoveSpeed = runSpeed;
                StartCoroutine(IncreaseSpeed());
                availableActions = HandleMovement;
                break;

            case state.DASHING:
                availableActions = HandleDash;
                break;

            case state.AIMING:
                StopCoroutine(IncreaseSpeed());
                targetMoveSpeed = aimingSpeed;
                StartCoroutine(DecreaseSpeed());
                availableActions = HandleMovement;
                break;
            case state.COOLDOWNING:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(dashOrigin, 1);
    }

    public bool IsWalking()
    {
        return isWalking;
    }


    private void HandleDash()
    {

        float dashDistance = Vector3.Distance(transform.position, dashOrigin);
        Vector3 currentPosition = transform.position;

        CharacterMove(moveDir, dashSpeed, Undashable);

        if (dashDistance > dashLenght || currentPosition == lastPosition)
        {
            SetState(previosState);
            dashCooldownTimer = 0f;
        }
        lastPosition = currentPosition;

    }
   
    private void HandleMovement()
    {
        Vector2 inputVector = inputManager.GetKeyboardMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        CharacterMove(moveDir, actualMoveSpeed, collideWith);
        CanDash();
    }


    private bool CanDash()
    {
        dashCooldownTimer += Time.deltaTime;
        if (dashCooldownTimer > dashCooldown)
        {
            return true;
        }
        return false;

    }

    public void TakeDamage(float DMG)
    {
        health -= DMG;
    }

    public int GetEquipedDrone()
    {
        return (int)equipedGunDrone;
    }

    public Transform GetGunDroneAnchorPoint()
    {
        return gunDroneAnchorPoint;
    }

    public int GetPlayerNumber()
    {
        return player;
    }

    public IHasHealth GetIHasHealth()
    {
        return this;
    }


    IEnumerator IncreaseSpeed()
    {
        for (float actualMoveSpeed = this.actualMoveSpeed; actualMoveSpeed < targetMoveSpeed; actualMoveSpeed += speedChangeSpeed * 0.1f)
        {
            this.actualMoveSpeed = actualMoveSpeed;
            yield return null;
        }
    }
    IEnumerator DecreaseSpeed()
    {
        for (float actualMoveSpeed = this.actualMoveSpeed; actualMoveSpeed < targetMoveSpeed; actualMoveSpeed -= speedChangeSpeed * 0.1f)
        {
            this.actualMoveSpeed = actualMoveSpeed;
            yield return null;
        }
    }

    private void CharacterMove(Vector2 inputVector, float moveSpeed, LayerMask UndashableLayer)
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerheight = 1.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerheight, playerRadius, moveDir, moveDistance, UndashableLayer);

        if (!canMove)
        {

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerheight, playerRadius, moveDirX, moveDistance, UndashableLayer);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0f && !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerheight, playerRadius, moveDirZ, moveDistance, UndashableLayer);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {

                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;

        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }

        
    }
}



