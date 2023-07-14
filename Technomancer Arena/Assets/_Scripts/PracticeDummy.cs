using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeDummy : MonoBehaviour, IHasHealth
{
    [SerializeField] private float health = 100f;

    private void Start()
    {
        health = 100f;
    }



    public void TakeDamage(float DMG)
    {
        Debug.Log(DMG);
        health -= DMG;
    }
}
