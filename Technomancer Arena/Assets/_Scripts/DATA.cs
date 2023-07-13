using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATA : MonoBehaviour
{

    public static DATA Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameInput gameInput;
    public MousePosition mousePosition;
    public Transform gunDroneAnchorPoint;
    public Camera mainCam;
}
