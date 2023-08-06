using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GunDroneSO : ScriptableObject
{
    public new string name;
    public GameObject dronePrefab, hitFX;
    public LayerMask hitLayerMask;
    public float damage;
    public float fireRate;
    public float aimingSpeed;
    public float range;
    public float shootHitboxRadius;

}
