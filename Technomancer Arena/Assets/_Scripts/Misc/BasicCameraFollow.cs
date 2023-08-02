using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPosition, player;
    [SerializeField] private float lerpPoint;
    private MousePosition mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = DATA.Instance.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = mousePosition.GetPointerTransform();

        transform.position = Vector3.Lerp(player.position, targetPosition.position, lerpPoint);
    }
}
