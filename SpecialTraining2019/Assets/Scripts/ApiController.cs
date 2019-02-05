using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ApiController : MonoBehaviour
{
    public const int TYPE_GET = 0;
    public const int TYPE_POST = 1;

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

    public IEnumerator HttpRequestAsync(string request, int type, IRequestCallback callback)
    {
        UnityWebRequest www;
        if (type == TYPE_GET)
        {
            www = UnityWebRequest.Get("http://54.183.173.15/api/" + request);
            yield return www.SendWebRequest();
        }
        else
        {
            string[] splited = request.Split('?');
            if (splited.Length != 2)
                yield break;
            www = UnityWebRequest.Post("http://54.183.173.15/api/" + splited[0], splited[1]);
            yield return www.SendWebRequest();
        }

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
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

}

public interface IRequestCallback
{
    void OnFinish(string response);
}