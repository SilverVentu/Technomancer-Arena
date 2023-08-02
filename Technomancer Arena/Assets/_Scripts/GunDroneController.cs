using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class GunDroneController : MonoBehaviour
{
    [SerializeField] private Transform[] dronePosition;
    [SerializeField] private float droneRotSpeed;
    [SerializeField] private LayerMask targetsLayer;
    [SerializeField] private GunDroneSO gunDroneSO;
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    private Collider[] nearbyEnemies;

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
        //transform.forward = Vector3.Slerp(transform.forward, aimDirection - transform.position, droneRotSpeed * Time.deltaTime);
        transform.LookAt(mousePosition.GetPointerTransform().position);
        gizmoPosition = mousePositionDirection.normalized + transform.root.position;
    }

    private void HandleDroneIdle()
    {
        //float distance = gunDroneSO.range;
        //Vector3 target = transform.forward;
        nearbyEnemies = Physics.OverlapSphere(transform.root.position, gunDroneSO.range, targetsLayer);
        if (nearbyEnemies.Length == 0) return;

        Collider target = nearbyEnemies[0];

        Debug.Log("enemies in range");

        for(int i = 0; i < nearbyEnemies.Length; i++)
        {
            if (nearbyEnemies[i].gameObject.layer == 7)
            {
                target = nearbyEnemies[i];
                transform.LookAt(target.transform);
                i = nearbyEnemies.Length;
                return;
            }

            if (Vector3.Distance(nearbyEnemies[i].transform.position, transform.root.position) <= Vector3.Distance(target.transform.position, transform.root.position))
            {
                target = nearbyEnemies[i];
            }

        }

        transform.LookAt(target.transform);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gizmoPosition, 1.5f);
        Gizmos.DrawWireSphere(transform.root.position, gunDroneSO.range);
    }
 
}
