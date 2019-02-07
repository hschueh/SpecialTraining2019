using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

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
        {
            //...set this one to be it...
            instance = this;
            //...otherwise...
            initAds();
        }
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }


    static Sprite profilePic = null;
    // Start is called before the first frame update
    void Start()
    {
        if(profilePic != null)
        {
            Image image = GameObject.Find("ProfileImg").GetComponent<Image>();
            image.sprite = profilePic;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void initAds()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-2737592620983884~9160375192";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-8550386526282187~1691404125";
        #else
            string appId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
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

        profilePic = Sprite.Create((Texture2D)texture, image.sprite.rect, image.sprite.pivot);

        image.sprite = profilePic;
    }
}
