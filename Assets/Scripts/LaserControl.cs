using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    private LineRenderer lineLaser; //  Line Renderer attached to the OVRController Prefab, so I can "see" the RayCast.
    // Start is called before the first frame update
    void Start()
    {
        lineLaser = GetComponent<LineRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        lineLaser.SetPosition(0, transform.position + transform.forward * 0.25f); // Adding this +transform.forward offset so that you can't "RayCast Pick-Up" an object that's too close. Going to use Colliders and Grip button for that.
        lineLaser.SetPosition(1, transform.position + transform.forward * 2); // Positions the line renderer to shoot from the controller (with a small offset), in forward direction.
    }
}
