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
        var Treasure2 = Resources.Load<GameObject>("Resources/Treasure_2");
        var Treasure5 = Resources.Load<GameObject>("Resources/Treasure_5");
        var Treasure10 = Resources.Load<GameObject>("Resources/Treasure_10");

        inventoryDic.Add(Treasure2, 0);
        inventoryDic.Add(Treasure5, 0);
        inventoryDic.Add(Treasure10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
