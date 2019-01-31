using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Text mainText;

    private bool gameStart;
    private int score;
    GameObject projectile;

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
        createProjectile();

        score++;

        mainText.text = "Score: " + score.ToString();
    }

    void createProjectile()
    {
        if (projectile == null)
            projectile = GameObject.Find("Projectile");
        Projectile projectileCopy = (Projectile)Instantiate(projectile).GetComponent<Projectile>();
        projectileCopy.SetSpeed(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        projectileCopy.SetPosition(0, 0);
        projectileCopy.Shot();
    }
}
