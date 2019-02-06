using UnityEngine;
using System.Collections;

public class ToastController : MonoBehaviour
{
    string data;//Toast要显示的数据
    AndroidJavaObject currentActivity;
    AndroidJavaClass UnityPlayer;
    AndroidJavaObject context;

    public static ToastController instance;
    public static ToastController getInstance()
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

            if (Application.platform == RuntimePlatform.Android)
            {
                UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            }
        }
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    void Start()
    {
    }

    public void SetToast(string data)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            this.data = data;
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(ShowToast));
        }
    }

    void ShowToast()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
            AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", data);
            AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
            toast.Call("show");
        }
    }
}