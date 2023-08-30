using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePointController : MonoBehaviour
{
    [SerializeField] private float capturePointProgress = 100, captureRate;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private MapControlManager mapControlManager;

    private bool playerInside = false, pointCaptured =false;

    
    // Start is called before the first frame update
    void Start()
    {
        mapControlManager = DATA.Instance.mapControlManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointCaptured) { return; }

        if (playerInside)
        {
            capturePointProgress -= captureRate * Time.deltaTime;
        }
        else
        {
            capturePointProgress += captureRate * 2 * Time.deltaTime;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(0, capturePointProgress);

        capturePointProgress = Mathf.Clamp(capturePointProgress, 0, 100);
        if(capturePointProgress == 0) 
        {
            pointCaptured = true;
            mapControlManager.AddCapturedPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            playerInside = false;
        }
    }
}
