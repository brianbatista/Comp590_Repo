using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreasureHunter : MonoBehaviour
{
    public Collectible[] treasureList;
    public TreasureHunterInventory inventory;
    public TextMesh totalScore;

    public TextMesh winCond;

    public int TreasureScore;

    private void Start() 
    {
        winCond.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        totalScore.text = "" + TreasureScore;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!inventory.treasure[0])
            {
            inventory.treasure[0] = treasureList[0];
            TreasureScore += inventory.treasure[0].treasureValue;
            }

            else
            {
                Debug.Log("Treasure 1 already collected.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!inventory.treasure[1])
            {
            inventory.treasure[1] = treasureList[1];
            TreasureScore += inventory.treasure[1].treasureValue;
            }
            else
            {
                Debug.Log("Treasure 2 already collected.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!inventory.treasure[2])
            {
            inventory.treasure[2] = treasureList[2];
            TreasureScore += inventory.treasure[2].treasureValue;
            }
            else
            {
                Debug.Log("Treasure 3 already collected.");
            }
        }

        if (inventory.treasure[0] && inventory.treasure[1] && inventory.treasure[2])
            {
                winCond.text = "YOU WIN!!!!";
            }
        }
}
