using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PortalCamera : MonoBehaviour
{
    public Transform playerCam, portal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angularDiff = Quaternion.Angle(transform.rotation, playerCam.rotation);
        Quaternion rotationDiff = Quaternion.AngleAxis(angularDiff, Vector3.up);
        Vector3 cameraRotation = angularDiff * playerCam.forward;
        transform.rotation = Quaternion.LookRotation(cameraRotation, Vector3.up);
    }
}
