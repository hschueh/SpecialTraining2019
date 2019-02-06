using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ApiController : MonoBehaviour
{
    public const int TYPE_GET = 0;
    public const int TYPE_POST = 1;
    public const string URL = "http://54.183.173.15/api/";

    public static ApiController instance;
    public static ApiController getInstance()
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
        DontDestroyOnLoad(instance);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator HttpRequestAsync(string request, Dictionary<string, string> parameters, int type, IRequestCallback callback)
    {
        UnityWebRequest www;
        if (type == TYPE_GET)
        {
            www = UnityWebRequest.Get(URL + request + BuildRequestString(parameters));
            yield return www.SendWebRequest();
        }
        else
        {
            www = UnityWebRequest.Post(URL + request, parameters);
            yield return www.SendWebRequest();
        }

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            if (callback != null)
                callback.OnFinish(www.downloadHandler.text);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            if(callback != null)
                callback.OnFinish(www.downloadHandler.text);
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }

    private string BuildRequestString(Dictionary<string, string> dicts)
    {
        string s = "";
        bool flag = true;
        foreach (KeyValuePair<string, string> pairs in dicts)
        {
            if (flag) s += "?";
            else s += "&";
            flag = false;
            s += pairs.Key;
            s += "=";
            s += pairs.Value;
        }
        return s;
    }
}

public interface IRequestCallback
{
    void OnFinish(string response);
}