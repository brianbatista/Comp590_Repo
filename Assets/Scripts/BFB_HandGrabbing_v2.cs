using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BFB_HandGrabbing_v2 : MonoBehaviour
{
/*
    public float _range;

    private LayerMask _shootableMask;

    // Saving for later, when I start reworking the inventory.
    //public TextMesh pointText;
    //public Inventory_v3 inventorySpace;

    public OVRInput.Button triggerPressed; // Set to Primary Index Trigger (Left Hand) or Secondary Index Trigger (Right Hand)
    
    public OVRInput.Button getAorX; // Set to X (Left Hand) or A (Right Hand)

    public OVRInput.Controller myController; // Set to L Touch (Left Hand) or R Touch (Right Hand)

    private GameObject _grabbed;

    private GameObject magneticHit;

    public bool objectGrabbed = false;

    public LineRenderer lineLaser; //  Line Renderer attached to the OVRController Prefab, so I can "see" the RayCast.

    // Start is called before the first frame update
    void Start()
    {
        _shootableMask = LayerMask.GetMask("Treasure"); // LayerMask for the collectible objects
        _range = 100f; // Range for the Raycast to shoot from the hand.
        lineLaser = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineLaser.SetPosition(0,transform.position + transform.forward); // Adding this +transform.forward offset so that you can't "RayCast Pick-Up" an object that's too close. Going to use Colliders and Grip button for that.
        lineLaser.SetPosition(1,transform.position + transform.forward * 2); // Positions the line renderer to shoot from the controller (with a small offset), in forward direction.
    
        //
        if(OVRInput.GetDown(triggerPressed, myController) && !objectGrabbed)
        {
            Ray ray = new Ray(transform.position + transform.forward * 1, transform.forward * _range); // Actual Ray
            Debug.DrawRay(transform.position + transform.forward * 1, transform.forward * _range, Color.green); // Ray Debugger
            RaycastHit hit = new RaycastHit();

            // Raycast Grabbing
            if (Physics.Raycast(ray, out hit, _range))
            {
                lineLaser.SetPosition(1,hit.point); // Sets Line Renderer to the object that was hit.
                GameObject pointed = hit.collider.gameObject; // Created so that you CAN point your laser at another object, but not manipulate it if you try to grab it while having something else grabbed already.

                if(pointed.layer == LayerMask.NameToLayer("Treasure"))
                {
                    Debug.Log("<color=red>OBJECT GRABBED</color> <color=green>USING DISTANCE SNAP GRAB </color>.");

                    objectGrabbed = true;
                    pointed.GetComponent<Rigidbody>().useGravity = false; // So it doesn't fall.
                    pointed.GetComponent<Rigidbody>().isKinematic = true; // So it doesn't collide with anything and acquire force.
                    pointed.transform.parent = this.transform; // Makes it "obey" the controller.
                    _grabbed = pointed;
                    pointed = null;
                    _grabbed.transform.localPosition = Vector3.zero; // Teleports it to your hand.
                }
            }

        // Raycast Grabbing
        if (Physics.Raycast(ray, out hit, _range))
        {
            lineLaser.SetPosition(1,hit.point); // Sets Line Renderer to the object that was hit.

            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Treasure"))
            {
                //
                // Using DISTANCE SNAP GRAB
                //
                // If controller set in Inspector presses Trigger
                if(OVRInput.GetDown(triggerPressed, myController) && !objectGrabbed)
                {
                    Debug.Log("<color=red>OBJECT GRABBED</color> <color=green>USING DISTANCE SNAP GRAB </color>.");

                    objectGrabbed = true;
                    pointed.GetComponent<Rigidbody>().useGravity = false; // So it doesn't fall.
                    pointed.GetComponent<Rigidbody>().isKinematic = true; // So it doesn't collide with anything and acquire force.
                    pointed.transform.parent = this.transform; // Makes it "obey" the controller.
                    _grabbed = pointed;
                    pointed = null;
                    _grabbed.transform.localPosition = Vector3.zero; // Teleports it to your hand.
                }

                //
                // Using DISTANCE NO-SNAP GRAB
                //
                // If controller set in Inspector presses A
                if(OVRInput.GetDown(getAorX, myController))
                {
                    if(!objectGrabbed)
                    {
                    Debug.Log("<color=red>OBJECT GRABBED</color> <color=green>USING DISTANCE NO-SNAP GRAB </color>.");

                    objectGrabbed = true;
                    pointed.GetComponent<Rigidbody>().useGravity = false;
                    pointed.GetComponent<Rigidbody>().isKinematic = true;
                    pointed.transform.parent = this.transform;
                    _grabbed = pointed;
                    pointed = null;
                    }

                    // Press again to release.
                    else if (objectGrabbed)
                    {
                    Debug.Log("<color=blue>OBJECT RELEASED.</color>");

                    objectGrabbed = false;
                    _grabbed.GetComponent<Rigidbody>().useGravity = true;
                    _grabbed.GetComponent<Rigidbody>().isKinematic = false;
                    
                    _grabbed.transform.parent = null;
                    _grabbed = null; 
                    }
                }
            }
        }
        // If Raycast hits ABSOLUTELY NOTHING (literally has to be pointed up)
        else
        {
            if(OVRInput.GetDown(triggerPressed, myController) && !objectGrabbed)
                {
                    Debug.Log("<color=red>OBJECT GRABBED</color> <color=green>USING MAGNETIC GRAB </color>.");

                    if(Physics.SphereCast(transform.position, 50f, transform.forward, out hit, 10))
                    {
                        magneticHit = hit.collider.gameObject;
                        objectGrabbed = true;
                        magneticHit.GetComponent<Rigidbody>().useGravity = false;
                        magneticHit.GetComponent<Rigidbody>().isKinematic = true;
                        magneticHit.transform.parent = this.transform;
                        _grabbed = magneticHit;
                        magneticHit = null;
                        _grabbed.transform.localPosition = Vector3.zero;
                    }
                }
        }


        // Releases the object.
        if(OVRInput.GetUp(triggerPressed, myController) && objectGrabbed && _grabbed)
            {
                Debug.Log("<color=blue>OBJECT RELEASED.</color>");

                objectGrabbed = false;
                _grabbed.GetComponent<Rigidbody>().useGravity = true;
                _grabbed.GetComponent<Rigidbody>().isKinematic = false;
                
                _grabbed.transform.parent = null;
                _grabbed = null;
            }

            /* Put in inventory.
            GameObject wasHit = Resources.Load<GameObject>(hit.collider.gameObject.GetComponent<Collectible>().ID);
            if(inventorySpace.inventoryDic.ContainsKey(wasHit))
                {
                    inventorySpace.inventoryDic[wasHit]++;
                }
                else
                {
                    inventorySpace.inventoryDic.Add(wasHit, 1);
                }

                Destroy(hit.collider.gameObject);
            */
        }
