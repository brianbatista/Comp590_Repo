using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BFB_HandGrab_v3 : MonoBehaviour
{
    public float _range;

    private LayerMask _shootableMask;

    private GameObject[] _grabbed = new GameObject[] {null, null};
    
    public GameObject[] controllers;
    private OVRInput.RawButton[] controllerTriggers = new OVRInput.RawButton[] {OVRInput.RawButton.LIndexTrigger,OVRInput.RawButton.RIndexTrigger};
    private OVRInput.RawButton[] controllerFaceButtons= new OVRInput.RawButton[] {OVRInput.RawButton.X,OVRInput.RawButton.A};

    // Start is called before the first frame update
    void Start()
    {
        _shootableMask = LayerMask.GetMask("Treasure"); // LayerMask for the collectible objects
        _range = 100f; // Range for the Raycast to shoot from the hand.
    }

    // Update is called once per frame
    void Update()
    {     
        //for each controller
        for (int i=0;i<2;i++){

            //              Ray Debugger, if needed. Can be seen in Editor, not in game.
            // Debug.DrawRay(controllers[i].transform.position + controllers[i].transform.forward * 1, controllers[i].transform.forward * _range, Color.green); 

            //Ray from this controller
            Ray ray = new Ray(controllers[i].transform.position + controllers[i].transform.forward * 1, controllers[i].transform.forward * _range); // Actual Ray
            RaycastHit hit = new RaycastHit();

            // Raycast Grabbing
            if (Physics.Raycast(ray, out hit, _range,_shootableMask))
            {
                Debug.Log("<color=red>Raycast has hit </color>" + hit.collider.gameObject);
     
                if(OVRInput.Get(controllerTriggers[i]))
                {
                    Debug.Log("<color=red>Trigger pressed on </color>" + hit.collider.gameObject);
                    Debug.Log("Controller index i is" + i);
                    //snap grab for this controller
                    grab(i,hit.collider.gameObject,true);
                }

                if(OVRInput.Get(controllerFaceButtons[i]))
                {
                    Debug.Log("<color=red>A/X pressed on </color>" + hit.collider.gameObject);
                    //nosnap grab for this controller
                    grab(i,hit.collider.gameObject,false);  
                }
            }
            else
            {
                if(OVRInput.Get(controllerTriggers[i]))
                {
                    //I don't think spherecast necessarily returns the closest object.... acts strange for objects of different sizes. 
                    //this is why I had a minimizer function
                    RaycastHit magneticHit;
                    //this should probably use controller positions.... which again can be a 2-element array
                    if(Physics.SphereCast(controllers[i].transform.position, 50f, controllers[i].transform.forward, out magneticHit, 10))
                    {
                        Debug.Log("<color=red>Magnetic Sphere casted.</color>");
                        //magnetic grab for this controller
                        if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Treasure"))
                        {
                            grab(i,magneticHit.collider.gameObject,true);
                            Debug.Log("<color=red>Magnetic Sphere collided with </color>" + magneticHit.collider.gameObject);
                        }
                    }
                }
            }

            //let go for this controller (since let go will behave the same regardless of how it was grabbed)
            if(OVRInput.GetUp(controllerTriggers[i]) || OVRInput.GetUp(controllerFaceButtons[i]))
            {
                Debug.Log("<color=red>Trigger or Face Button was released.</color>");

                if (_grabbed[i]){

                    Debug.Log("<color=blue>OBJECT RELEASED.</color>");
                    _grabbed[i].GetComponent<Rigidbody>().useGravity = true;
                    _grabbed[i].GetComponent<Rigidbody>().isKinematic = false;
                    
                    _grabbed[i].transform.parent = null;
                    _grabbed[i] = null;
                }
            }
        }
    }

    void grab(int whichController, GameObject thingToGrab, bool shouldSnap){
        
        Debug.Log("<color=red>_grabbed</color> " + whichController + " <color=red>is</color> " + _grabbed[whichController]);

        if (!_grabbed[whichController]){
            Debug.Log("<color=red>OBJECT GRABBED</color> <color=green>USING DISTANCE SNAP GRAB </color>.");

            thingToGrab.GetComponent<Rigidbody>().useGravity = false; // So it doesn't fall.
            thingToGrab.GetComponent<Rigidbody>().isKinematic = true; // So it doesn't collide with anything and acquire force.
            thingToGrab.transform.parent = controllers[whichController].transform; // Makes it "obey" the controller.
            _grabbed[whichController] = thingToGrab;
            if (shouldSnap){
                _grabbed[whichController].transform.localPosition = Vector3.zero; // Teleports it to your hand.
            } 
            else{

            }
        }
    }
}
