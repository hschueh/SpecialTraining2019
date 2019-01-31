using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Text mainText;

    private bool gameStart;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        gameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            gameStart = true;
        }


        if (gameStart == false)
        {
            return;
        }

        score++;

        mainText.text = "Score: " + score.ToString();
    }
}
