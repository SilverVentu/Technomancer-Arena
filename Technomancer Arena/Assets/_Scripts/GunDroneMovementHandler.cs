using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GunDroneMovementHandler : MonoBehaviour{

    [SerializeField] private float droneSpeed,droneRotSpeed;
    [SerializeField] private Vector3 anchorOffset;
    private Transform gunDroneAnchorPoint;
    private MousePosition mousePosition;
    private Vector3 gizmoPosition, gizmoPosition2;

    public Vector3 aimDir;

    private void Start()
    {
        gunDroneAnchorPoint = DATA.Instance.gunDroneAnchorPoint;
        mousePosition = DATA.Instance.mousePosition;
    }


    void Update(){

        Vector3 mousePositionDirection = (mousePosition.GetGunPointerTransform().position + anchorOffset) - transform.position;


        // this normalices the position of the mouse, then adds the position of the drone and substracts its height
        Vector3 aimDirection = mousePositionDirection.normalized + DATA.Instance.gunDroneAnchorPoint.transform.position + anchorOffset;

        transform.position = Vector3.Slerp(transform.position, gunDroneAnchorPoint.position + anchorOffset, Time.deltaTime * droneSpeed);
        transform.forward = aimDirection - transform.position;

        gizmoPosition = mousePositionDirection.normalized + DATA.Instance.gunDroneAnchorPoint.transform.position;
        gizmoPosition2 = aimDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gizmoPosition, .5f);
    }
}
