using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int treasureValue;

    private void Start() 
    {
        treasureValue = Random.Range(1, 10);
    }
}
