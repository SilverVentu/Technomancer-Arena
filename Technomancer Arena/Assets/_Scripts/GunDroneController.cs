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
    private Transform targetPosition;

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

        transform.localPosition = targetPosition.position;
        availableDroneActions();
    }

    private void SetState(state state)
    {

        switch (state)
        {
            case state.IDLE:
                targetPosition = dronePosition[0];
                availableDroneActions = HandleDroneIdle;
                break;

            case state.AIMING:
                targetPosition = dronePosition[1];
                availableDroneActions = HandleDroneAim;
                break;
        }
    }
    private void HandleDroneAim()
    {
        
        Vector3 mousePositionDirection = mousePosition.GetPointerTransform().position - targetPosition.position;

        // this normalices the position of the mouse, then adds the position of the drone and substracts its height
        Vector3 aimDirection = mousePositionDirection.normalized + transform.position;
        transform.position = mousePositionDirection.normalized + targetPosition.position;
        transform.forward = Vector3.Slerp(transform.forward, mousePosition.GetPointerTransform().position - transform.position, droneRotSpeed * Time.deltaTime);
        //transform.LookAt(mousePosition.GetPointerTransform().position);
        gizmoPosition = mousePositionDirection.normalized + transform.root.position;
    }

    private void HandleDroneIdle()
    {
        transform.forward = transform.forward;
        nearbyEnemies = Physics.OverlapSphere(targetPosition.position, gunDroneSO.range, targetsLayer);

        if (nearbyEnemies.Length == 0) return;

        Collider target = nearbyEnemies[0];

        Debug.Log("enemies in range");

        for(int i = 0; i < nearbyEnemies.Length; i++)
        {
            if (nearbyEnemies[i].gameObject.layer == 7)
            {
                target = nearbyEnemies[i];
                break;
            }
            Debug.Log("checkpoint 2");

            float currentCharacter = Vector3.Distance(nearbyEnemies[i].transform.position, transform.root.position);
            float targetedCharacter = Vector3.Distance(target.transform.position, transform.root.position);

            if (currentCharacter <= targetedCharacter)
            {
                target = nearbyEnemies[i];
            }

        }
        Debug.Log("out of the loop");
        transform.forward = Vector3.Slerp(transform.forward, target.transform.position - transform.position, gunDroneSO.aimingSpeed * Time.deltaTime);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gizmoPosition, 1.5f);
        Gizmos.DrawWireSphere(targetPosition.position, gunDroneSO.range);
    }
 
}
