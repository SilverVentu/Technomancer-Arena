using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunDroneVisualController : MonoBehaviour
{
    [SerializeField] private GameObject hitFX;
    [SerializeField] private Vector3 hitFXOffset;
    [SerializeField] private Animator gunDroneAnimator;
    [SerializeField] private GunDroneController gunDroneController;
    private InputManager gameInput;
    private GunDroneSO equipedGunDroneSO;
   
    private ParticleSystem hitFXParticles;

    private void Start()
    {
        gameInput = DATA.Instance.inputManager;
        //equipedGunDroneSO = gunDroneManager.gunDroneSO[0];
        //SpawnDrone(equipedGunDroneSO.droneVisual);

        /*if(gunDroneController.GetPlayerNumber() == 0)
        {
            
        }*/

        gunDroneController.OnShot += GunDroneController_OnShot;
        gunDroneAnimator = GetComponent<Animator>();
        hitFXParticles = hitFX.GetComponent<ParticleSystem>();
        hitFXParticles.Pause();

    }

    private void GunDroneController_OnShot(object sender, GunDroneController.OnHitEventArgs e)
    {
        hitFX.transform.position = e.hitPosition + hitFXOffset;
        hitFXParticles.Play();
        gunDroneAnimator.SetTrigger("Shoot");
    }

    


    /*private void SpawnDrone(GameObject drone)
    {
        GameObject gunDrone = Instantiate(drone, transform);
        gunDrone.transform.localPosition = new Vector3(0,0,-0.3f);
        

        if (hitFX != null)
        {
            Destroy(hitFX);
        }
        
    }*/
}
