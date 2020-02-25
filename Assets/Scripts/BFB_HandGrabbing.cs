using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BFB_HandGrabbing : MonoBehaviour
{
    public float _range;

    private LayerMask _shootableMask;

    //public TextMesh pointText;

    //public Inventory_v3 inventorySpace;

    public OVRInput.Button buttonPressed;
    public OVRInput.Controller myController;

    private GameObject _grabbed;

    public bool objectGrabbed = false;

    public LineRenderer lineLaser;

    // Start is called before the first frame update
    void Start()
    {
        _shootableMask = LayerMask.GetMask("Treasure");
        _range = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * _range, Color.green);

        lineLaser.SetPosition(0,transform.position);
        lineLaser.SetPosition(1,transform.position + transform.forward * 2);

        Ray ray = new Ray(transform.position, transform.forward * _range);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, _range))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Treasure"))
            {
                GameObject pointed = hit.collider.gameObject;

                // If controller set in Inspector presses Trigger
                if(OVRInput.GetDown(buttonPressed, myController) && !objectGrabbed)
                {
                    Debug.Log("<color=red>OBJECT GRABBED.</color>");

                    objectGrabbed = true;
                    pointed.GetComponent<Rigidbody>().useGravity = false;
                    pointed.GetComponent<Rigidbody>().isKinematic = true;
                    pointed.transform.parent = this.transform;
                    _grabbed = pointed;
                    pointed = null;
                    _grabbed.transform.localPosition = Vector3.zero;
                }
            }

        }

        if(OVRInput.GetUp(buttonPressed, myController) && objectGrabbed && _grabbed)
            {
                Debug.Log("<color=blue>OBJECT RELEASED.</color>");

                objectGrabbed = false;
                _grabbed.GetComponent<Rigidbody>().useGravity = true;
                _grabbed.GetComponent<Rigidbody>().isKinematic = false;
                
                // ????
                _grabbed.GetComponent<Rigidbody>().velocity = Vector3.zero;
                _grabbed.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                 
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
}
