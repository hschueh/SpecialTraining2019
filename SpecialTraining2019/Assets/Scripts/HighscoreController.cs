using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreController : MonoBehaviour
{
    public static int STATE_LOADING = 0;
    public static int STATE_FINISH = 1;

    public static int TYPE_GLOBAL = 0;
    public static int TYPE_SELF = 1;

    public static int BOARD_SIZE = 10;

    public Text mainText;
    public Text bodyText;
    private int boardType;
    private int boardState;

    private List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

    // Start is called before the first frame update
    void Start()
    {
        boardType = TYPE_SELF;
        RetrieveData();
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
        }
        else
        {
            bodyText.text = "loading...";
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
        RetrieveData();
    }

    void RetrieveData()
    {
        boardState = STATE_LOADING;
        Thread thread = new Thread(delegate ()
        {
            // Gen data. The process shuold change depends on boardType.
            list.Add(new KeyValuePair<string, int>("Hoso", 99999));
            list.Add(new KeyValuePair<string, int>("Hoso", 69999));
            boardState = STATE_FINISH;
        });
        //Start the Thread and execute the code inside it
        thread.Start();
    }

    public void GoToPlayScene()
    {
        Debug.Log("GoToPlayScene");
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }

}
