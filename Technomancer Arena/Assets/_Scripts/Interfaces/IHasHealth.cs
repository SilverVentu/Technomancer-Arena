using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasHealth
{
    public event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;


    public class OnDamageTakenEventArgs : EventArgs
    {
        public float DMG;
    }


    public void TakeDamage(float DMG);

}
