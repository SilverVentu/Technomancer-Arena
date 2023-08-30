using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapControlManager : MonoBehaviour
{
    [SerializeField] private int pointsCaptured = 0;
    [SerializeField] private float individeualCapturedPointsValue = .25f, portalProgress = 0;
    private CapturePointController capturePointController;


    private void Update()
    {
        portalProgress += pointsCaptured * individeualCapturedPointsValue * Time.deltaTime;
    }
    public void AddCapturedPoint()
    {
        pointsCaptured++;
    } 
    
    public void RemoveCapturedPoint()
    {
        pointsCaptured--;
    }
}
