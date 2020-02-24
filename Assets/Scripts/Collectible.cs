using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Collectible : MonoBehaviour
{
    public int treasureValue;

    public string ID;

    public TextMesh scoreValue;

    private void Start() 
    {
        scoreValue.text = "" + treasureValue;
        scoreValue.color = Color.black;
    }
}
