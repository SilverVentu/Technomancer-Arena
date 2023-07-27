using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldPointerController : MonoBehaviour
{
    [SerializeField ] private GameObject worldPointerPrefab;
    private MousePosition mousePosition;
    private GameObject worldPointer;

    private void Start()
    {
        worldPointer = Instantiate(worldPointerPrefab, transform.position, Quaternion.identity);
        mousePosition =DATA.Instance.mousePosition;
    }
    // Update is called once per frame
    void Update()
    {
        worldPointer.transform.position =mousePosition.GetPointerTransform().position;
    }
}
