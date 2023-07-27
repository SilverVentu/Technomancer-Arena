using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDroneController : MonoBehaviour
{
    [SerializeField] private Transform[] dronePosition;
    [SerializeField] private float droneRange;

    private Vector3 gizmoPosition;



    private InputManager inputManager;
    private MousePosition mousePosition;
    private delegate void AvailableDroneActions();

    private AvailableDroneActions availableDroneActions;

    private void Start()
    {
        inputManager = DATA.Instance.inputManager;
        mousePosition = DATA.Instance.mousePosition;
        inputManager.OnTakeAim += InputManager_OnTakeAim;
        inputManager.OnLowerAim += InputManager_OnLowerAim;
        SetState(state.IDLE);
    }

    private void InputManager_OnTakeAim(object sender, System.EventArgs e)
    {
        SetState(state.AIMING);
    }

    private void InputManager_OnLowerAim(object sender, System.EventArgs e)
    {
        SetState(state.IDLE);
    }


    private enum state
    {
        IDLE,
        AIMING,
    }

    private void Update()
    {
        availableDroneActions();
    }

    private void SetState(state state)
    {

        switch (state)
        {
            case state.IDLE:
                transform.localPosition = dronePosition[0].localPosition;
                availableDroneActions = HandleDroneIdle;
                break;

            case state.AIMING:
                //transform.localPosition = dronePosition[1].localPosition;
                availableDroneActions = HandleDroneAim;
                break;
        }
    }
    private void HandleDroneAim()
    {
        
        Vector3 mousePositionDirection = mousePosition.GetPointerTransform().position - transform.root.position;

        // this normalices the position of the mouse, then adds the position of the drone and substracts its height
        Vector3 aimDirection = mousePositionDirection.normalized + transform.position;
        transform.position = mousePositionDirection.normalized + transform.root.position;
        transform.forward = aimDirection - transform.position;
        gizmoPosition = mousePositionDirection.normalized + transform.root.position;
    }

    private void HandleDroneIdle()
    {
        Vector3 mousePositionDirection = (mousePosition.GetPointerTransform().position) - transform.position;

        // this normalices the position of the mouse, then adds the position of the drone and substracts its height
        Vector3 aimDirection = mousePositionDirection.normalized + transform.position;

        //transform.position = Vector3.Slerp(transform.position, gunDroneAnchorPoint.position + anchorOffset, Time.deltaTime * droneSpeed);
        transform.forward = aimDirection - transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gizmoPosition, 1.5f);
    }
 
}
