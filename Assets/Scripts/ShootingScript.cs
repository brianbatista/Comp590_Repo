using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    // public Collectible treasureGrabbed;
    public float _range;
    private Camera _camera;
    private LayerMask _shootableMask;

    public Inventory_v2 inventorySpace;

    public TextMesh pointText;
    int pointScore;

    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;

        _shootableMask = LayerMask.GetMask("Treasure");
        
        pointText.text = "0";

        // Tried automatically acquiring inventory
        // Inventory_v2 inventorySpace = this.transform.parent.gameObject.GetComponent(typeof(Inventory_v2)) as Inventory_v2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lineOrigin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        Debug.DrawRay(lineOrigin, _camera.transform.forward * _range, Color.green);

        if (Input.GetMouseButtonDown(0))
        {
            // Old way. What could I change Input.mousePosition to? Just adding lineOrigin there doesn't work.
            // Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            Ray ray1 = new Ray(lineOrigin, _camera.transform.forward * _range);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray1, out hit, _range, _shootableMask))
            {
                Debug.Log("hit " + hit.collider.gameObject);

                //inventorySpace.inventoryGO.Add(hit.collider.gameObject);
                GameObject wasHit = Resources.Load<GameObject>(hit.collider.gameObject.GetComponent<Collectible>().ID);

                if(inventorySpace.inventoryDic.ContainsKey(wasHit))
                {
                    inventorySpace.inventoryDic[wasHit]++;
                }
                else
                {
                    inventorySpace.inventoryDic.Add(wasHit, 1);
                }

                // Change score
                pointScore += hit.collider.gameObject.GetComponent<Collectible>().treasureValue;

                Destroy(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            inventorySpace.inventoryGO.Clear();
            pointScore = 0;
        }

    pointText.text = "" + pointScore;
    }
}
