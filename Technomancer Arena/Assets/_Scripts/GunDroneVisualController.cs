using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunDroneVisualController : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    private GunDroneManager gunDroneManager;
    private GunDroneSO equipedGunDroneSO;
    private Animator gunDroneAnimator;
    private GameObject hitFX;
    private ParticleSystem hitFXParticles;

    private void Start()
    {
        gunDroneManager = GetComponent<GunDroneManager>();
        //equipedGunDroneSO = gunDroneManager.gunDroneSO[0];
        //SpawnDrone(equipedGunDroneSO.droneVisual);
        //gunDroneManager.OnShot += GunDroneManager_OnShot;
        gameInput.OnPlayer1Attack += GameInput_OnAttack;
        
    }

    private void GameInput_OnAttack(object sender, System.EventArgs e)
    {
    }

    private void GunDroneManager_OnShot(object sender, GunDroneManager.OnHitEventArgs e)
    {

        hitFX.transform.position = e.hitPosition;
        hitFXParticles.Play();
        gunDroneAnimator.SetTrigger("Shoot");

    }


    /*private void SpawnDrone(GameObject drone)
    {
        GameObject gunDrone = Instantiate(drone, transform);
        gunDrone.transform.localPosition = new Vector3(0,0,-0.3f);
        gunDroneAnimator = gunDrone.GetComponent<Animator>();

        if (hitFX != null)
        {
            Destroy(hitFX);
        }
        hitFX = Instantiate(equipedGunDroneSO.hitFX, Vector3.zero, Quaternion.identity);
        hitFXParticles = hitFX.GetComponent<ParticleSystem>();
        hitFXParticles.Pause();
    }*/
}
