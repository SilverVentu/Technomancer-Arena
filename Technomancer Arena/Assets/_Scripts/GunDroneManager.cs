using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunDroneManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private MousePosition mousePosition;
    [SerializeField] private LayerMask targetLayer;
    private float fireRate;
    private const string ENEMY = "Enemy";
    public GunDroneSO[] gunDroneSO;

    public event EventHandler<OnHitEventArgs> OnShot;


    public class OnHitEventArgs : EventArgs
    {
        public Vector3 hitPosition;
    }

    private void Start()
    {
        
        gameInput.OnAttack += GameInput_OnAttack;
        //target = enemyTarget.GetComponent<IHasHealth>();
    }

    private void Update()
    {
        fireRate += Time.deltaTime;
    }

    private void GameInput_OnAttack(object sender, System.EventArgs e)
    {
        if(fireRate >= gunDroneSO[0].fireRate)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, gunDroneSO[0].range, targetLayer))
            {
                if (hit.transform.TryGetComponent<IHasHealth>(out IHasHealth target) /*&& mousePosition.GetGunPointerTransform().CompareTag(ENEMY)*/)
                {
                    target.TakeDamage(gunDroneSO[0].damage);
                    OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = hit.point});
                    fireRate = 0;
                }
                else
                {
                    OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = hit.point});
                    fireRate = 0;
                }

            }else
            {
                OnShot?.Invoke(this, new OnHitEventArgs { hitPosition = transform.forward *gunDroneSO[0].range});
            }

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * gunDroneSO[0].range);
    }
}
