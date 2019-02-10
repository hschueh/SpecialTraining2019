using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.MiniJSON;

public class HighscoreController : MonoBehaviour
{
    public static int STATE_IDLE = 0;
    public static int STATE_LOADING = 1;
    public static int STATE_FINISH = 2;

    public static int TYPE_GLOBAL = 0;
    public static int TYPE_SELF = 1;

    public static int BOARD_SIZE = 10;

    public Text mainText;
    public Text bodyText;
    private int boardType;
    static  int boardState;

    static HttpClient client = new HttpClient();

    static private List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

    private readonly GetLeaderboardCallback callback = new GetLeaderboardCallback();

    // Start is called before the first frame update
    void Start()
    {
        boardType = TYPE_SELF;
        boardState = STATE_LOADING;
        StartCoroutine(ApiController.getInstance().HttpRequestAsync("score.php", new Dictionary<string, string>(), ApiController.TYPE_GET, callback));

        #if UNITY_ANDROID
        GameObject.Find("Button").SetActive(false);
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (boardType == TYPE_SELF)
            mainText.text = "Local Leaderboard:";
        else
            mainText.text = "Global Leaderboard:";

        if (boardState == STATE_FINISH)
        {
            bodyText.text = GenerateLeaderboard();
            boardState = STATE_IDLE;
        }
        else if (boardState == STATE_LOADING)
        {
            bodyText.text = "loading...";
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            GoToMenuScene();
        }
    }

    string GenerateLeaderboard()
    {
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, int> entry in list)
        {
            sb.Append(entry.Key + ": " + entry.Value + '\n');
        }
        return sb.ToString();
    }

    public void SetBoardType(int boardType)
    {
        this.boardType = boardType;
        boardState = STATE_LOADING;
        StartCoroutine(ApiController.getInstance().HttpRequestAsync("score.php", new Dictionary<string, string>(), ApiController.TYPE_GET, callback));
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    class GetLeaderboardCallback : IRequestCallback
    {
        public void OnFinish(string response)
        {
            // {"result":"1","data":[{"id":"2","user_id":"123","score":"456"},{"id":"3","user_id":"123","score":"123"},{"id":"1","user_id":"1","score":"100"}]}
            var dict = Json.Deserialize(response) as Dictionary<string, object>;
            object data;
            if (dict.TryGetValue("data", out data))
            {
                foreach (object scores in (List<object>)data)
                {
                    int score = int.Parse((string)((Dictionary<string, object>)scores)["score"]);
                    string name = (string)((Dictionary<string, object>)scores)["username"];

                    list.Add(new KeyValuePair<string, int>(name, score));
                }
            }
            boardState = STATE_FINISH;
        }
    }
}
