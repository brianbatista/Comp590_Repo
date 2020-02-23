using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadmovementScript : MonoBehaviour
{
    public Camera playerCamera;

    public Vector3 targetAngles;

    private Vector3 followAngles;
    private Vector3 followVelocity;
    private Vector3 originalRotation;
    public float mouseSensitivity = 10;
    public float mouseSensitivityInternal;
    public  float fOVToMouseSensitivity = 1;
    float baseCamFOV;
    public float cameraSmoothing = 5f;

    public float verticalRotationRange = 170;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivityInternal = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseXInput;
        float mouseYInput;
        float camFOV = playerCamera.fieldOfView;
        mouseXInput = Input.GetAxis("Mouse Y");
        mouseYInput = Input.GetAxis("Mouse X");
        if(targetAngles.y > 180) { targetAngles.y -= 360; followAngles.y -= 360; } else if(targetAngles.y < -180) { targetAngles.y += 360; followAngles.y += 360; }
        if(targetAngles.x > 180) { targetAngles.x -= 360; followAngles.x -= 360; } else if(targetAngles.x < -180) { targetAngles.x += 360; followAngles.x += 360; }
        targetAngles.y += mouseYInput * (mouseSensitivityInternal - ((baseCamFOV-camFOV)*fOVToMouseSensitivity)/6f);
        targetAngles.x += mouseXInput * (mouseSensitivityInternal - ((baseCamFOV-camFOV)*fOVToMouseSensitivity)/6f);
        targetAngles.y = Mathf.Clamp(targetAngles.y, -0.5f * Mathf.Infinity, 0.5f * Mathf.Infinity);
        targetAngles.x = Mathf.Clamp(targetAngles.x, -0.5f * verticalRotationRange, 0.5f * verticalRotationRange);
        followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, (cameraSmoothing)/100);
        playerCamera.transform.localRotation = Quaternion.Euler(-followAngles.x + originalRotation.x,0,0);
        transform.localRotation =  Quaternion.Euler(0, followAngles.y+originalRotation.y, 0);
    }
}
