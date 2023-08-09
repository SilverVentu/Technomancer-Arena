using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using static GunDroneControllerOLD;

public class GunDroneController : MonoBehaviour
{
    [SerializeField] private Transform[] dronePosition;
    [SerializeField] private float aimAssistRadius, droneRotSpeed;
    [SerializeField] private LayerMask targetsLayer;
    [SerializeField] private GunDroneSO gunDroneSO;
    [SerializeField] private List<GameObject> targets = new List<GameObject>();


    private Collider[] nearbyEnemies;
    private Collider[] pointerNearbyEnemies;
    private Transform targetPosition;
    private float fireRate;
    private Vector3 gizmoPosition;
    private InputManager inputManager;
    private MousePosition mousePosition;
    private delegate void AvailableDroneActions();


    public event EventHandler<OnHitEventArgs> OnShot;

    private AvailableDroneActions availableDroneActions;

    private void Start()
    {
        inputManager = DATA.Instance.inputManager;
        mousePosition = DATA.Instance.mousePosition;
        inputManager.OnPlayerAttack += InputManager_OnPlayerAttack;
        inputManager.OnTakeAim += InputManager_OnTakeAim;
        inputManager.OnLowerAim += InputManager_OnLowerAim;
        SetState(state.IDLE);
    }

    private void InputManager_OnPlayerAttack(object sender, EventArgs e)
    {
        Shoot();

        Debug.Log("PewPew");
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
        fireRate += Time.deltaTime;
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

        pointerNearbyEnemies = Physics.OverlapSphere(mousePosition.GetPointerTransform().position, aimAssistRadius, targetsLayer);

        Vector3 assistedMousePosition = Vector3.Lerp(GetAveragePosition(pointerNearbyEnemies), mousePosition.GetPointerTransform().position, .5f);

        Vector3 mousePositionDirection = assistedMousePosition - targetPosition.position;

        // this normalices the position of the mouse, then adds the position of the drone and substracts its height
        Vector3 aimDirection = mousePositionDirection.normalized + transform.position;
        transform.position = mousePositionDirection.normalized + targetPosition.position;



        transform.forward = Vector3.Slerp(transform.forward, assistedMousePosition - transform.position, droneRotSpeed * Time.deltaTime);
        
        gizmoPosition = GetAveragePosition(pointerNearbyEnemies)/*mousePositionDirection.normalized + dronePosition[1].localPosition*/;
    }

    private void HandleDroneIdle()
    {
        transform.forward = transform.forward;
        nearbyEnemies = Physics.OverlapSphere(targetPosition.position, gunDroneSO.range, targetsLayer);

        if (nearbyEnemies.Length == 0) return;

        Collider target = nearbyEnemies[0];

        for (int i = 0; i < nearbyEnemies.Length; i++)
        {
            if (nearbyEnemies[i].gameObject.layer == 7)
            {
                target = nearbyEnemies[i];
                break;
            }

            float currentCharacter = Vector3.Distance(nearbyEnemies[i].transform.position, dronePosition[1].localPosition);
            float targetedCharacter = Vector3.Distance(target.transform.position, dronePosition[1].localPosition);

            if (currentCharacter <= targetedCharacter)
            {
                target = nearbyEnemies[i];
            }

        }
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(target.transform.position.x, 0, target.transform.position.z) - transform.position, gunDroneSO.aimingSpeed * Time.deltaTime);
    }


    private void Shoot()
    {
        if (fireRate >= gunDroneSO.fireRate && Physics.SphereCast(transform.localPosition, gunDroneSO.shootHitboxRadius, transform.forward, out RaycastHit hit, gunDroneSO.range, gunDroneSO.hitLayerMask))
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
            if (fireRate >= gunDroneSO.fireRate)
            {
                OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = (transform.forward * gunDroneSO.range) + transform.localPosition });
                fireRate = 0;

            }
        }
    }

    private Vector3 GetAveragePosition(Collider[] entries)
    {
        if (entries.Length == 0)
        {
            return mousePosition.GetPointerTransform().position;
        }

        var bounds = new Bounds(entries[0].transform.position, Vector3.zero);
        
        foreach (var entry in entries)
        {
            bounds.Encapsulate(entry.transform.position);
        }

        return bounds.center;
    }


    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(targetPosition.position, gunDroneSO.range);
        Gizmos.DrawWireSphere(mousePosition.GetPointerTransform().position, aimAssistRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gizmoPosition, 1.5f);
    }*/

}
