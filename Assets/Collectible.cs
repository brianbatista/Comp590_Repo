using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Collectible : MonoBehaviour
{
    public int treasureValue;

    //[SerializeField]
    //public TextElement fuckit;

    public TextMesh scoreValue;

    private void Start() 
    {
        treasureValue = Random.Range(1, 10);
        scoreValue.text = "" + treasureValue;
    }
}
