using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Timeline;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour, IHasHealth
{

    public static PlayerManager Instance { get; private set; }

    [SerializeField] private int player;
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 7f, dashSpeed, dashLenght, dashCooldown;
    [SerializeField] private LayerMask Undashable, collideWith;
    [SerializeField] private GunDroneOptions equipedGunDrone;
    [SerializeField] private Transform gunDroneAnchorPoint;
    [SerializeField] private state currentState;

    private GameInput gameInput;
    private bool isWalking;
    private Vector3 lastDirectionDir, moveDir, dashOrigin, lastPosition;
    private float moveDistance, /*dashDistance = 0,*/ dashCooldownTimer = 0;

    delegate Vector2 GetInputVector();
    //delegate void GetInputAction();

    List<GetInputVector> getInputVector = new List<GetInputVector>();
    //List<GetInputAction> getInputAction = new List<GetInputAction>();

    private enum state
    {
        NEUTRAL,
        DASHING,
        COOLDOWNING,
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        Instance = this;
        gameInput = DATA.Instance.gameInput;
    }

    private void Start()
    {
        getInputVector.Add(gameInput.GetKeyboardMovementVectorNormalized);
        getInputVector.Add(gameInput.GetJoystickMovementVectorNormalized);

        gameInput.OnPlayer1Attack += GameInput_OnPlayer1Attack;
        gameInput.OnPlayer2Attack += GameInput_OnPlayer2Attack;
        gameInput.OnPlayer1Dash += GameInput_OnPlayer1Dash;
        gameInput.OnPlayer2Dash += GameInput_OnPlayer2Dash;
    }


    private void GameInput_OnPlayer1Attack(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
    private void GameInput_OnPlayer2Attack(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
    private void GameInput_OnPlayer1Dash(object sender, GameInput.OnPlayer1DashEventArgs e)
    {
        if(player == e.player && CanDash())
        {
            dashOrigin = transform.position;
            currentState = state.DASHING;
        }
    }
    private void GameInput_OnPlayer2Dash(object sender, GameInput.OnPlayer2DashEventArgs e)
    {
        if (player == e.player && CanDash())
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

        CanMove(moveDir, dashSpeed, Undashable);

        if (dashDistance > dashLenght || currentPosition == lastPosition)
        {
            currentState = state.NEUTRAL;
            dashCooldownTimer = 0f;
        }
        lastPosition = currentPosition;

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


   
    private void HandleMovement()
    {
        Vector2 inputVector = getInputVector[player]();

        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        CanMove(moveDir, moveSpeed, collideWith);
        CanDash();
    }

    public void TakeDamage(float DMG)
    {
        health -= DMG;
    }


    public enum GunDroneOptions
    {
        SPADA = 0, SCOPETA = 1,
    }

    public int GetEquipedDrone()
    {
        return (int)equipedGunDrone;
    }

    public Transform GetGunDroneAnchorPoint()
    {
        return gunDroneAnchorPoint;
    }

    private void CanMove(Vector2 inputVector, float moveSpeed, LayerMask UndashableLayer)
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



