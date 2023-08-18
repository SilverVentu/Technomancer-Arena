using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPosition, player;
    [SerializeField] private float lerpPoint, cameraFollowSpeed;
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

        Vector3 playerMouseMidPoint = Vector3.Lerp(player.position, targetPosition.position, lerpPoint);

        transform.position = Vector3.Lerp(transform.position, playerMouseMidPoint, cameraFollowSpeed * Time.deltaTime);
    }
}
