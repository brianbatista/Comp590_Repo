using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    public int scoreValue;

    private TextMesh scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 0;
        scoreText = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "<color=red>" + scoreValue + "</color>";
    }
}
