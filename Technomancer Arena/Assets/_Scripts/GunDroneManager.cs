using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class GunDroneManager : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private PlayerManager[] players;
    [SerializeField] private GunDroneSO[] gunDroneSO;    

    public class OnHitEventArgs : EventArgs
    {
        public Vector3 hitPosition;
    }

    private void Start()
    {
        /*gameInput = DATA.Instance.gameInput;
        mousePosition = DATA.Instance.mousePosition;
        gameInput.OnPlayer1Attack += GameInput_OnAttack;
        //target = enemyTarget.GetComponent<IHasHealth>();*/

        foreach(PlayerManager playerManager in players)
        {
            SpawnDrone(playerManager.GetEquipedDrone(),playerManager.GetGunDroneAnchorPoint());
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * gunDroneSO[0].range);
    }

    private void SpawnDrone (int droneIndex, Transform gunDroneAnchorPoint)
    {
        GameObject gunDrone = Instantiate(gunDroneSO[droneIndex].dronePrefab);
        gunDrone.GetComponent<GunDroneController>().SetGunDroneAnchorPoint(gunDroneAnchorPoint);

    }
}
