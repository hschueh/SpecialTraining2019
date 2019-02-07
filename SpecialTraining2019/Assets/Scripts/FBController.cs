using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Facebook.Unity;
using Facebook.MiniJSON;

public class FBController : MonoBehaviour
{
    public static FBController instance;
    public static FBController getInstance()
    {
        return instance;
    }

    private string userId = "";
    private string username = "";

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

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            Debug.Log("Initialize the Facebook SDK!");
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            LoginToFacebook(false);
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            alreadyLoggedIn();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void alreadyLoggedIn()
    {
        // AccessToken class will have session details
        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
        // Print current access token's User ID
        Debug.Log("FB userid: " + aToken.UserId);
        this.userId = aToken.UserId;
        // Print current access token's granted permissions
        foreach (string perm in aToken.Permissions)
        {
            Debug.Log(perm);
        }
        FB.API("me?fields=id,name,picture", HttpMethod.GET, HandleFacebookDelegate);
    }

    public void LoginToFacebook(bool popDialog = true)
    {

        if (FB.IsLoggedIn)
        {
            Debug.Log("Already logged in.");
            alreadyLoggedIn();
        }
        else if (popDialog)
        {
            // Facebook
            Debug.Log("Login to FB...");
            var perms = new List<string>() { };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        else
        {
            Debug.Log("Isn't logged in. No need to pop dialog");
        }
    }

    public string GetUserId()
    {
        return userId;
    }

    public string GetUsername()
    {
        return username;
    }

    static IEnumerator ReadImageToObject(string path)
    {
        //https://platform-lookaside.fbsbx.com/platform/profilepic/?asid=10215711139469634&height=50&width=50&ext=1551866220&hash=AeTiWYxKJqvTizos

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            MenuController.getInstance().SetProfilePic(((DownloadHandlerTexture)www.downloadHandler).texture);
        }
    }

    void HandleFacebookDelegate(IGraphResult result)
    {
        Debug.Log(result);
        //string url = result.ResultDictionary["picture"]["data"]["url"];

        var dict = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
        object picture;
        if (dict.TryGetValue("picture", out picture))
        {
            object data = ((Dictionary<string, object>)picture)["data"];
            string url = (string)((Dictionary<string, object>)data)["url"];
            StartCoroutine(ReadImageToObject(url));
        }

        if (dict.TryGetValue("name", out string name))
        {
            username = name;
        }
    }

}
