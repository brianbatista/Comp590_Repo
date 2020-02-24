using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_v2 : MonoBehaviour
{
    public List<GameObject> inventoryGO = new List<GameObject>();

    // Dictionary<TKey, TValue>
    public Dictionary<GameObject, int> inventoryDic = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
