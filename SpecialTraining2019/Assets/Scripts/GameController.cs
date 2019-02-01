using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static int STATE_STOP = 0;
    public static int STATE_START = 1;
    public static int STATE_PAUSE = 2;
    public static int STATE_DEAD = 3;

    public static GameController instance;

    public Text mainText;
    private int gameState;
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
        gameState = STATE_STOP;

        projectTileList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (gameState == STATE_STOP)
            {
                StartGame();
            }
            else if (gameState == STATE_START)
            {
                // PauseGame();
            }
            else if (gameState == STATE_PAUSE)
            {
                // ResumeGame();
            }

        }

        if (gameState == STATE_STOP)
        {
            // do nothing
            return;
        } else if (gameState == STATE_START)
        {
            // create one bullet
            CreateProjectile();

            score++;
            mainText.text = "Score: " + score.ToString();
        } else if (gameState == STATE_PAUSE)
        {
            // do nothing
            return;
        } else if (gameState == STATE_DEAD)
        {
            GameOver();
        }


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
        Debug.Log("Start Game");
        gameState = STATE_START;
        score = 0;
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        gameState = STATE_PAUSE;
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        gameState = STATE_START;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        gameState = STATE_STOP;
        mainText.text = "Game over!! Score: " + score.ToString();
    }

    public int GetGameState()
    {
        return gameState;
    }
}
