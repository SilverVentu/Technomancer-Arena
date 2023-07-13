using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GunDroneMovementHandler : MonoBehaviour{

    [SerializeField] private Transform gunDroneAnchorPoint;
    [SerializeField] private float droneSpeed, anchorOffset, droneRotSpeed;
    [SerializeField] private MousePosition mousePosition;
    public Vector3 aimDir;
    
    void Update(){

        Vector3 aimDirection = (mousePosition.GetGunPointerTransform().position + new Vector3(0f,anchorOffset,0f)) - transform.position;
        //Debug.DrawRay(transform.position, aimDirection, Color.black);

        transform.position = Vector3.Slerp(transform.position, gunDroneAnchorPoint.position + new Vector3(0f, anchorOffset,0f), Time.deltaTime * droneSpeed);
        transform.forward = Vector3.Lerp(transform.forward, aimDirection.normalized, Time.deltaTime * droneRotSpeed);

    }
}
