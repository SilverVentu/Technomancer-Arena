using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePosition : MonoBehaviour{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    private Transform gunPointer;

    private void Start()
    {
        gunPointer = GetComponent<Transform>();
    }
    void Update(){

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit raycastHit, float.MaxValue, layerMask)){

            //gunPointer = raycastHit.transform;
            gunPointer.position = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
            gunPointer.tag = raycastHit.transform.tag;
        }
        
    }

    public Transform GetGunPointerTransform(){
        return gunPointer; 
    }
}
