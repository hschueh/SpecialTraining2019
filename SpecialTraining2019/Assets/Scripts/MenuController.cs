using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GoToPlayScene()
    {
        Debug.Log("GoToPlayScene");
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }

    public void GoToLeaderboardScene()
    {
        Debug.Log("GoToLeaderboard triggered");
        SceneManager.LoadScene("LeaderboardScene", LoadSceneMode.Single);
    }

    public void LoginByFacebook()
    {
        FBController.getInstance().LoginToFacebook();
    }
}
