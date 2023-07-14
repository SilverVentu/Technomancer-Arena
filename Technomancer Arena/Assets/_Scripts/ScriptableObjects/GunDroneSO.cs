using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GunDroneSO : ScriptableObject
{
    public string name;
    public GameObject droneVisual, hitFX;
    public float damage;
    public float fireRate;
    public float range;

}
