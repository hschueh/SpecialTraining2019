using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
    private int boardState;

    static HttpClient client = new HttpClient();

    private List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

    // Start is called before the first frame update
    void Start()
    {
        boardType = TYPE_SELF;
        StartCoroutine(RetrieveDataAsync());
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

        if(Input.GetKey(KeyCode.Escape))
        {
            GoToMenuScene();
        }
    }

    string GenerateLeaderboard()
    {
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, int> entry in list)
        {
            sb.Append(entry.Key + ": " + entry.Value+ '\n');
        }
        return sb.ToString();
    }

    public void SetBoardType(int boardType)
    {
        this.boardType = boardType;
        StartCoroutine(RetrieveDataAsync());
    }

    IEnumerator RetrieveDataAsync()
    {
        boardState = STATE_LOADING;

        UnityWebRequest www = UnityWebRequest.Get("http://54.183.173.15/api/test.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;

            list.Add(new KeyValuePair<string, int>(www.downloadHandler.text, 1));
            list.Add(new KeyValuePair<string, int>("Hoso", 2));
            boardState = STATE_FINISH;
        }
    }


    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

}
