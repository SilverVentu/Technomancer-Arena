using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeDummy : MonoBehaviour, IHasHealth
{
    [SerializeField] private float health = 100f;

    public event EventHandler<IHasHealth.OnDamageTakenEventArgs> OnDamageTaken;

    private void Start()
    {
        OnDamageTaken += PracticeDummy_OnDamageTaken;
        health = 100f;
    }

    private void PracticeDummy_OnDamageTaken(object sender, IHasHealth.OnDamageTakenEventArgs e)
    {
        Debug.Log("you dealt " + e.DMG + " to " + this);
        
    }


    public void TakeDamage(float DMG)
    {
        Debug.Log(DMG);
        health -= DMG;
        OnDamageTaken?.Invoke(this, new IHasHealth.OnDamageTakenEventArgs { DMG = DMG });
    }
}
