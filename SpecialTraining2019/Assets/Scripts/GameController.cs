using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Text mainText;
    private bool gameStart;
    private int score;

    private ArrayList projectTileList;

    GameObject projectile;

    public static GameController getInstance()
    {
        return instance;
    }

    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        gameStart = false;

        projectTileList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (gameStart == false)
            {
                StartGame();
            }
            else
            {
                // PauseGame();
            }

        }

        if (gameStart == false)
        {
            return;
        }

        CreateProjectile();
        score++;
        mainText.text = "Score: " + score.ToString();
    }

    void CreateProjectile()
    {
        if (projectile == null)
            projectile = GameObject.Find("Projectile");

        Projectile projectileCopy = (Projectile)Instantiate(projectile).GetComponent<Projectile>();
        projectileCopy.SetSpeed(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        projectileCopy.SetPosition(0, 0);
        projectileCopy.Shot();

        projectTileList.Add(projectileCopy);

    }

    public void StartGame()
    {
        gameStart = true;
        score = 0;
    }

    public void PauseGame()
    {
        // (TODO) change gameStart from bool to enum to describe state.
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        mainText.text = "Game over!! Score: " + score.ToString();
        gameStart = false;
    }

    public bool IsGameStart()
    {
        return gameStart;
    }
}
