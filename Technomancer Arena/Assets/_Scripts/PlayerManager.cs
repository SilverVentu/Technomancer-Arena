using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance { get; private set; }

    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    private Vector3 lastDirectionDir;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        Instance = this;
    }


    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }



    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastDirectionDir = moveDir;
        }

    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerheight = 1.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerheight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerheight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0f && !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerheight, playerRadius, moveDirZ, moveDistance);

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


