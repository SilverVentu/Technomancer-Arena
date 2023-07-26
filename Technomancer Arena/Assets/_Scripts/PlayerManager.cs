using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IHasHealth
{
    [SerializeField] private int player;
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 7f, dashSpeed, dashLenght, dashCooldown;
    [SerializeField] private LayerMask Undashable, collideWith;
    [SerializeField] private GunDroneOptions equipedGunDrone;
    [SerializeField] private Transform gunDroneAnchorPoint;
    [SerializeField] private state currentState;

    private KeyboardInput gameInput;
    private bool isWalking;
    private Vector3 lastDirectionDir, moveDir, dashOrigin, lastPosition;
    private float moveDistance, dashCooldownTimer = 0;

    private enum state
    {
        NEUTRAL,
        DASHING,
        COOLDOWNING,
    }


    private void Awake()
    {
        gameInput = DATA.Instance.gameInput;
    }

    private void Start()
    {
        gameInput.OnPlayerDash += GameInput_OnPlayerDash;
    }

    private void GameInput_OnPlayerDash(object sender, EventArgs e)
    {
        if (CanDash())
        {
            dashOrigin = transform.position;
            currentState = state.DASHING;
        }
    }



    private void Update()
    {

        switch (currentState)
        {
            case state.NEUTRAL:
                HandleMovement();
                break;

            case state.DASHING:
                HandleDash();
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
            currentState = state.NEUTRAL;
            dashCooldownTimer = 0f;
        }
        lastPosition = currentPosition;

    }
   
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetKeyboardMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        CharacterMove(moveDir, moveSpeed, collideWith);
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
    public void SetHealthTo(float healthAmount)
    {
        health = healthAmount;
    }


    public enum GunDroneOptions
    {
        SPADA, SCOPETA, SPADA2
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



