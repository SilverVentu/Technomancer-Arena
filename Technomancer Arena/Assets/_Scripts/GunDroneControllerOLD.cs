using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static GunDroneManagerOLD;
using static UnityEngine.GraphicsBuffer;

public class GunDroneControllerOLD : MonoBehaviour{

    [SerializeField] private float droneSpeed, droneRotSpeed, castRadius;
    [SerializeField] private Vector3 anchorOffset;
    [SerializeField] private GunDroneSO gunDroneSO;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int player;
    private IHasHealth playerIHasHealth;
    private InputManager gameInput;
    private Transform playerPosition;
    [SerializeField]private Transform gunDroneAnchorPoint;
    private MousePosition mousePosition;
    private Vector3 gizmoPosition, gizmoPosition2, mainHit, auxHit;
    private float fireRate;


    public event EventHandler<OnHitEventArgs> OnShot;

    public class OnHitEventArgs : EventArgs
    {
        public Vector3 hitPosition;
    }

    public Vector3 aimDir;

    private void Start()
    {
        mousePosition = DATA.Instance.mousePosition;
        gameInput = DATA.Instance.inputManager;



        gameInput.OnPlayerAttack += GameInput_OnPlayer1Attack;

    }

    private void GameInput_OnPlayer1Attack(object sender, EventArgs e)
    {
        Shoot();
        Debug.Log("PewPew");
    }

    private void GameInput_OnPlayer2Attack(object sender, EventArgs e)
    {
        Shoot();
    }

    void Update(){

        fireRate += Time.deltaTime;
        HandleDroneAim();


            
    }

    private void HandleDroneAim()
    {
        Vector3 mousePositionDirection = (mousePosition.GetPointerTransform().position) - transform.position;

        // this normalices the position of the mouse, then adds the position of the drone and substracts its height
        Vector3 aimDirection = mousePositionDirection.normalized + transform.position;

        //transform.position = Vector3.Slerp(transform.position, gunDroneAnchorPoint.position + anchorOffset, Time.deltaTime * droneSpeed);
        transform.forward = aimDirection - transform.position;

        gizmoPosition = mousePositionDirection.normalized + transform.position;
        gizmoPosition2 = aimDirection;
    }
    
    public void SetGunDroneAnchorPoint(Transform anchorPoint)
    {
        gunDroneAnchorPoint = anchorPoint;
    }
    public void SetGunDronePlayer(int playerNumber)
    {
        player = playerNumber;
    }

    public int GetPlayerNumber()
    {
        return player;
    }

    public void SetPlayerPosition(Transform playerPos)
    {
        playerPosition = playerPos;                                                                                                                                                                              
    }

    public void SetPlayerIHasHealth(IHasHealth iHasHealth)
    {
        playerIHasHealth = iHasHealth;
    }

    private void Shoot()
    {
        if (fireRate >= gunDroneSO.fireRate && Physics.SphereCast(transform.position, castRadius, transform.forward, out RaycastHit hit, gunDroneSO.range, targetLayer))
        {
            if (hit.transform.TryGetComponent<IHasHealth>(out IHasHealth target))
            {
                Debug.Log(target);
                target.TakeDamage(gunDroneSO.damage);
                OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = hit.point });
                fireRate = 0;
            }
            else
            {
                OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = hit.point });
                fireRate = 0;
            }
        }
        else
        {
            if(fireRate >= gunDroneSO.fireRate)
            {
                OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = transform.forward * gunDroneSO.range });
                fireRate = 0;

            }
        }
    }



    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(gizmoPosition, .5f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward * gunDroneSO.range);
        Gizmos.DrawWireSphere(mainHit, 1f);
        /*Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(auxHit, .5f);
        Gizmos.DrawRay(GetAuxRayOrigin(), GetAuxRayDirection());*/
    }

    /*private Vector3 GetAuxRayOrigin()
    {
        return playerPosition.position + transform.forward * 1f;
    }
    private Vector3 GetAuxRayDirection()
    {
        return (mainHit - GetAuxRayOrigin()).normalized * Vector3.Distance(GetAuxRayOrigin(), GetMainRayRangeTip());
    }
    
    private Vector3 GetMainRayRangeTip()
    {
        return gunDroneAnchorPoint.position + transform.forward * gunDroneSO.range;
    }*/

}
