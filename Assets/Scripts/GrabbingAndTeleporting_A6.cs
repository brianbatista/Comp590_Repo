using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GrabbingAndTeleporting_A6 : MonoBehaviour
{
    private float _range;

    private LayerMask _shootableMask;

    private GameObject[] _grabbed = new GameObject[] {null, null};
    
    [Header("0 = Left Controller / 1 = Right Controller")]
    public GameObject[] controllers = new GameObject[2];
    private OVRInput.RawButton[] controllerTriggers = new OVRInput.RawButton[] {OVRInput.RawButton.LIndexTrigger,OVRInput.RawButton.RIndexTrigger};
    private OVRInput.RawButton[] controllerFaceButtons= new OVRInput.RawButton[] {OVRInput.RawButton.X,OVRInput.RawButton.A};
    private OVRInput.RawButton[] controllerGrips = new OVRInput.RawButton[] {OVRInput.RawButton.LHandTrigger,OVRInput.RawButton.RHandTrigger}; // Not used :)
    private OVRInput.RawButton teleportButton = OVRInput.RawButton.RThumbstickUp;

    public BeltInventory inventoryBelt;

    public GameObject scoreKeeper;

    // Start is called before the first frame update
    void Start()
    {
        _shootableMask = LayerMask.GetMask("Treasure"); // LayerMask for the collectible objects
        _range = 100f; // Range for the Raycast to shoot from the hand.
    }

    void Update() 
    {    
        for (int i=0; i<2; i++)
        {
            // Trigger-Ray Grab
            if(OVRInput.Get(controllerTriggers[i]))
            {
                Ray ray = new Ray(controllers[i].transform.position + controllers[i].transform.forward * 1, controllers[i].transform.forward * _range); // Actual Ray
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, _range,_shootableMask))
                {
                    Debug.Log("<color=red>Trigger Grab </color> has hit " + hit.collider.gameObject);
                    grab(i,hit.collider.gameObject,true);
                }
            }

            // A/X Distant Grab
            if(OVRInput.Get(controllerFaceButtons[i]))
            {
                Ray ray = new Ray(controllers[i].transform.position + controllers[i].transform.forward * 1, controllers[i].transform.forward * _range); // Actual Ray
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, _range,_shootableMask))
                {
                    Debug.Log("<color=red>FaceButton Grab </color> has hit " + hit.collider.gameObject);
                    grab(i,hit.collider.gameObject,false);
                }
            }
            
            // Release
            if(OVRInput.GetUp(controllerTriggers[i]) || OVRInput.GetUp(controllerFaceButtons[i]))
            {
                Debug.Log("<color=red>Trigger or Face Button was released.</color>");

                if (_grabbed[i])
                {
                    // Drop into Inventory
                    var capsuleOff = new Vector3 (0.5f, 0, 0);
                    Collider[] hits = Physics.OverlapCapsule(inventoryBelt.transform.position - capsuleOff, inventoryBelt.transform.position + capsuleOff, 0.15f, _shootableMask, QueryTriggerInteraction.Collide);
                    
                    // If item is dropped inside Inventory
                    foreach(Collider inventoryHit in hits)
                    {
                        //if (inventoryHit.tag == "Treasure")
                        //{
                            scoreKeeper.GetComponent<ScoreUpdater>().scoreValue += inventoryHit.GetComponent<Collectible>().treasureValue;
                            var wasHit = Resources.Load<GameObject>(inventoryHit.gameObject.GetComponent<Collectible>().ID);
                            if(inventoryBelt.inventoryDic.ContainsKey(wasHit))
                            {
                                inventoryBelt.inventoryDic[wasHit]++;
                            }
                            else
                            {
                                inventoryBelt.inventoryDic.Add(wasHit, 1);
                            }

                            Destroy(inventoryHit.gameObject);
                            wasHit = null;
                        //}
                    }

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
