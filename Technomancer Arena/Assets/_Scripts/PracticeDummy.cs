using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeDummy : MonoBehaviour, IHasHealth
{
    [SerializeField] private float health = 100f;
    [SerializeField] private GameObject alivePrefab, deadPrefab;

    private void Start()
    {
        health = 100f;
    }
    private void Update()
    {
        if(health <= 0)
        {
            alivePrefab.SetActive(false);
            deadPrefab.SetActive(true);
        }
    }


    public void TakeDamage(float DMG)
    {
        Debug.Log(DMG);
        health -= DMG;
    }
}
