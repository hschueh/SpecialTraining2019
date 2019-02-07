using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using Facebook.MiniJSON;
using System.Security.Cryptography;
using System.Text;
using GoogleMobileAds.Api;

public class GameController : MonoBehaviour
{

    public static int STATE_STOP = 0;
    public static int STATE_START = 1;
    public static int STATE_PAUSE = 2;
    public static int STATE_DEAD = 3;
    public static int STATE_PRESTART = 4;
    public static int STATE_SHOWAD = 5;

    public static int BULLET_LIMIT = 20;

    public static GameController instance;

    public Text mainText;
    public Text scoreText;
    public Text scoreText2;

    private int gameState;
    private int score;
    private float gameStartTime;

    private int prestart_counter;
    private int counter;
    private int game_counter;

    GameObject projectile;
    GameObject player;
    private ProjectController projectController;

    private readonly UploadScoreCallback callback = new UploadScoreCallback();

    private InterstitialAd interstitial;

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
        gameStartTime = Time.time;
        game_counter = 0;

        if (projectile == null)
            projectile = GameObject.Find("Projectile");
        if (player == null)
            player = GameObject.Find("Player");

        projectile.transform.position = new Vector3(1, 2, -10);

        projectController = new ProjectController(projectile);

        mainText.text = "Touch screen to start.";
        scoreText.gameObject.SetActive(false);
        scoreText2.gameObject.SetActive(false);

        RequestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GoToMenuScene();
        }

        if (Input.GetKey(KeyCode.Space) || (Input.touchCount > 0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
        {
            if (gameState == STATE_STOP)
            {
                prestart_counter = 0;
                player.SetActive(true);
                Debug.Log("Prestart " + prestart_counter);
                gameState = STATE_PRESTART;
                PreStartGame();
            }
            else if (gameState == STATE_PRESTART)
            {  
                // do nothing
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

            } else if (gameState == STATE_SHOWAD)
            {
                // do nothing
            }
        }



        if (gameState == STATE_STOP)
        {
            // do nothing
            GameStop();
            return;
        } else if (gameState == STATE_PRESTART)
        {
            prestart_counter++;
            mainText.gameObject.SetActive(false);
            PreStartGame();
        } else if (gameState == STATE_START)
        {
            // create one bullet
            if (counter % 3 == 0) {
                CreateProjectile();
            }

            float diff_time = Time.time - gameStartTime;

            scoreText.text = ((int)diff_time).ToString();
            scoreText2.text = String.Format(".{0}", (int)((diff_time - (int)diff_time) * 1000));

            counter++;
            score++;

        } else if (gameState == STATE_PAUSE)
        {
            // do nothing
            return;
        } else if (gameState == STATE_DEAD)
        {
            // do nothing
            GameDead();
            return;
        } else if (gameState == STATE_SHOWAD)
        {
            ShowAds();
        }

    }

    void CreateProjectile()
    {
        projectController.StartProject();
    }

    public void PreStartGame()
    {
        Vector3 pos = new Vector3(0, -10.0f + prestart_counter  / 3.0f, 0);

        player.transform.position = pos;

        if (prestart_counter >= 30)
        {
            prestart_counter = 0;
            StartGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        gameState = STATE_START;
        score = 0;
        counter = 0;
        game_counter++;
        gameStartTime = Time.time;
        mainText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        scoreText2.gameObject.SetActive(true);

        if (interstitial != null)
        {
            interstitial.Destroy();
        }
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
        mainText.gameObject.SetActive(true);
        mainText.text = "Game over!!";
        UploadScore();
    }

    public void GameStop()
    {
        player.SetActive(false);
        mainText.text = "Touch screen to start.";
    }

    public void GameDead()
    {
        if (projectController.GetBulletNumber() == 0)
        {
            if (game_counter % 1 == 0)
            {
                gameState = STATE_SHOWAD;
            }
            else
            {
                gameState = STATE_STOP;
            }
        }
        mainText.text = "Game over!!";
    }

    public void ShowAds()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            RequestInterstitial();
        }

        gameState = STATE_STOP;
    }

    public int GetGameState()
    {
        return gameState;
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void UploadScore()
    {
        string user_id = FBController.getInstance().GetUserId();
        string username = FBController.getInstance().GetUsername();
        if (user_id == "")
        {
            user_id = "0";
            username = "Anomynous";
        }

        StartCoroutine(ApiController.getInstance().HttpRequestAsync(
            "score.php",
            new Dictionary<string, string>(){
                    { "user_id",  user_id},
                    { "score", "" + score},
                    { "username",  username}
                },
            ApiController.TYPE_POST, callback));
    }

    public Vector3 getPlayerPos()
    {
        return player.transform.position;
    }

    public int GetCounter()
    {
        return counter;
    }

    public float GetTime()
    {
        return Time.time - gameStartTime;
    }

    private void RequestInterstitial()
    {
    #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2737592620983884/3026211688";
    #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-8550386526282187/3705594880";
    #else
            string adUnitId = "unexpected_platform";
    #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    class UploadScoreCallback : IRequestCallback
    {
        public void OnFinish(string response)
        {
            if(!response.Equals("\"1\""))
            {

                ToastController.getInstance().SetToast("Share score failed!");
            }
        }
    }
}
