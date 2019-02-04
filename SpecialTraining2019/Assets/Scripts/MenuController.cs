using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public static MenuController getInstance()
    {
        return instance;
    }

    private void Awake()
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
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

    public void SetProfilePic(Texture texture)
    {
        Image image = GameObject.Find("ProfileImg").GetComponent<Image>();

        image.sprite = Sprite.Create((Texture2D)texture, image.sprite.rect, image.sprite.pivot);
    }
}
