using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDroneManagerOLD : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private PlayerManager[] players;
    [SerializeField] private GunDroneSO[] gunDroneSO;    


    private void Start()
    {
        /*gameInput = DATA.Instance.gameInput;
        mousePosition = DATA.Instance.mousePosition;
        gameInput.OnPlayer1Attack += GameInput_OnAttack;
        //target = enemyTarget.GetComponent<IHasHealth>();*/

        foreach(PlayerManager playerManager in players)
        {
            SpawnDrone(playerManager);
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * gunDroneSO[0].range);
    }

    private void SpawnDrone (PlayerManager playerManager)
    {
        GameObject gunDrone = Instantiate(gunDroneSO[playerManager.GetEquipedDrone()].dronePrefab);
        GunDroneControllerOLD gunDroneController = gunDrone.GetComponent<GunDroneControllerOLD>();

        gunDroneController.SetGunDroneAnchorPoint(playerManager.GetGunDroneAnchorPoint());
        gunDroneController.SetGunDronePlayer(playerManager.GetPlayerNumber());
        gunDroneController.SetPlayerPosition(players[playerManager.GetPlayerNumber()].transform);
        gunDroneController.SetPlayerIHasHealth(playerManager.GetIHasHealth());
    }
}
