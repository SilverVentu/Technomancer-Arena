using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunDroneController : MonoBehaviour
{
    [SerializeField] private Transform[] dronePosition;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float aimAssistRadius, droneRotSpeed;
    [SerializeField] private LayerMask targetsLayer;
    [SerializeField] private GunDroneSO gunDroneSO;
    [SerializeField] private List<GameObject> targets = new List<GameObject>();


    private Collider[] nearbyEnemies, pointerNearbyEnemies;
    private Collider target, previousTarget;
    private Transform targetPosition;
    private float fireRate, currentCharacterDistance, targetedCharacterDistance;
    private Vector3 gizmoPosition, aimingStartPoint, aimingEndPoint;
    private InputManager inputManager;
    private MousePosition mousePosition;
    private delegate void AvailableDroneActions();
    private bool isPressingShootButton = false;
    private event EventHandler OnNewTarget;

    public class OnHitEventArgs : EventArgs
    {
        public Vector3 hitPosition;
    }

    public event EventHandler<OnHitEventArgs> OnShot;

    private AvailableDroneActions availableDroneActions;

    private void Start()
    {
        inputManager = DATA.Instance.inputManager;
        mousePosition = DATA.Instance.mousePosition;
        inputManager.OnPlayerShoot += InputManager_OnPlayerShoot;
        inputManager.OnPlayerStopShooting += InputManager_OnPlayerStopShooting;
        inputManager.OnTakeAim += InputManager_OnTakeAim;
        inputManager.OnLowerAim += InputManager_OnLowerAim;
        OnNewTarget += GunDroneController_OnNewTarget;
        SetState(state.IDLE);
    }

    private void GunDroneController_OnNewTarget(object sender, EventArgs e)
    {
        StartCoroutine(AimLinearlyTowards());
    }

    private void InputManager_OnPlayerStopShooting(object sender, EventArgs e)
    {
        isPressingShootButton = false;
    }

    private void InputManager_OnPlayerShoot(object sender, EventArgs e)
    {
        isPressingShootButton = true;

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
    private enum idleState
    {
        NOENEMIESNEAR,
        ENEMIESNEAR,
    }

    private void Update()
    {

        transform.localPosition = targetPosition.position;
        availableDroneActions();
        fireRate += Time.deltaTime;
        if (isPressingShootButton)
        {
            Shoot();
        }
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


        //target = nearbyEnemies[0];
        if (nearbyEnemies.Length == 0)
        {
            return;
        }


        currentCharacterDistance = 0;
        targetedCharacterDistance = gunDroneSO.range;

        for (int i = 0; i < nearbyEnemies.Length; i++)
        {
            currentCharacterDistance = Vector3.Distance(nearbyEnemies[i].transform.position, playerPosition.position);
            
            if (currentCharacterDistance <= targetedCharacterDistance)
            {
                Debug.Log("new target");
                target = nearbyEnemies[i];
            }

            if (target != null)
            {
                float v = Vector3.Distance(target.transform.position, playerPosition.position);
                targetedCharacterDistance = v;
            }

        }

        if (target == null)
        { 
            return;
        }
        else if(target != previousTarget)
        {
            previousTarget = target;
            Debug.Log("New Target Acquired");
            aimingStartPoint = transform.forward - transform.position;
            aimingEndPoint = new Vector3(target.transform.position.x, 0, target.transform.position.z) - transform.position;
            OnNewTarget?.Invoke(this, EventArgs.Empty);            
        }
        
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

    private IEnumerator AimLinearlyTowards()
    {
        for (float currentPos = 0f; currentPos < 1; currentPos += gunDroneSO.aimingSpeed * Time.deltaTime)
        {
            transform.forward = Vector3.Slerp(aimingStartPoint, aimingEndPoint, currentPos);
            Debug.Log("aiming");

            yield return null;
        }
        Debug.Log("Broke");
        yield break;
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
