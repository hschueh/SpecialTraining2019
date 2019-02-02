using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static int STATE_STOP = 0;
    public static int STATE_START = 1;
    public static int STATE_PAUSE = 2;
    public static int STATE_DEAD = 3;

    public static int BULLET_LIMIT = 30;

    public static GameController instance;

    public Text mainText;
    private int gameState;
    private int score;

    private int counter;

    GameObject projectile;
    private ProjectController projectController;

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

        if (projectile == null)
            projectile = GameObject.Find("Projectile");

        projectile.transform.position = new Vector3(1, 2, -10);

        projectController = new ProjectController(projectile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.touchCount > 0)
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
            else if (gameState == STATE_DEAD)
            {
                if (projectController.GetBulletNumber() == 0)
                {
                    gameState = STATE_STOP;
                }
            }
        }



        if (gameState == STATE_STOP)
        {
            // do nothing
            return;
        } else if (gameState == STATE_START)
        {
            // create one bullet
            if (counter % 3 == 0) {
                CreateProjectile();
            }

            counter++;
            score++;
            mainText.text = "Score: " + score.ToString();
        } else if (gameState == STATE_PAUSE)
        {
            // do nothing
            return;
        } else if (gameState == STATE_DEAD)
        {
            // do nothing
            return;
        }


    }

    void CreateProjectile()
    {
        projectController.StartProject();
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        gameState = STATE_START;
        score = 0;
        counter = 0;
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
        gameState = STATE_DEAD;
        mainText.text = "Game over!!\n Score: " + score.ToString();
    }

    public int GetGameState()
    {
        return gameState;
    }

    public void GoToLeaderboard()
    {
        Debug.Log("GoToLeaderboard triggered");
        SceneManager.LoadScene("LeaderboardScene", LoadSceneMode.Single);
    }
}
