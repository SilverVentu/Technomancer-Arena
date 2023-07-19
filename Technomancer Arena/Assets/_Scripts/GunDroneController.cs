using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using static GunDroneManager;
using static UnityEngine.GraphicsBuffer;

public class GunDroneController : MonoBehaviour{

    [SerializeField] private float droneSpeed,droneRotSpeed;
    [SerializeField] private Vector3 anchorOffset;
    [SerializeField] private GunDroneSO gunDroneSO;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int player;
    private Transform gunDroneAnchorPoint;
    private MousePosition mousePosition;
    private Vector3 gizmoPosition, gizmoPosition2;
    private float fireRate;


    public event EventHandler<OnHitEventArgs> OnShot;

    public Vector3 aimDir;

    private void Start()
    {
        mousePosition = DATA.Instance.mousePosition;
    }    


    void Update(){

        fireRate -= Time.deltaTime;
        HandleDroneAim();
        
    }

    private void GameInput_OnAttack(object sender, System.EventArgs e)
    {
        if (fireRate >= gunDroneSO.fireRate)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, gunDroneSO.range, targetLayer))
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
                OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = transform.forward * gunDroneSO.range });
            }

        }
    }

    private void HandleDroneAim()
    {
        if(player == 0)
        {
            
            Vector3 mousePositionDirection = (mousePosition.GetGunPointerTransform().position + anchorOffset) - transform.position;

            // this normalices the position of the mouse, then adds the position of the drone and substracts its height
            Vector3 aimDirection = mousePositionDirection.normalized + gunDroneAnchorPoint.transform.position + anchorOffset;

            transform.position = Vector3.Slerp(transform.position, gunDroneAnchorPoint.position + anchorOffset, Time.deltaTime * droneSpeed);
            transform.forward = aimDirection - transform.position;

            gizmoPosition = mousePositionDirection.normalized + gunDroneAnchorPoint.transform.position;
            gizmoPosition2 = aimDirection;
        }
        else
        {

            Vector2 stickPosition = DATA.Instance.gameInput.GetJoystickAimDirectionVectorNormalized();
            Vector3 aimDirection = new Vector3(stickPosition.x, 0, stickPosition.y) +  transform.position;

            //Debug.Log(stickPosition);

            transform.position = Vector3.Slerp(transform.position, gunDroneAnchorPoint.position + anchorOffset, Time.deltaTime * droneSpeed);
            if(stickPosition != Vector2.zero)
            {
                transform.forward = new Vector3(stickPosition.x, 0, stickPosition.y)/*aimDirection - transform.position*/;
            }


            gizmoPosition = aimDirection;

        }
        



    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gizmoPosition, .5f);
    }

    public void SetGunDroneAnchorPoint(Transform anchorPoint)
    {
        gunDroneAnchorPoint = anchorPoint;
    }
}
