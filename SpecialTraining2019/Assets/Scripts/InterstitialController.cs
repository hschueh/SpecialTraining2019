using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class InterstitialController : MonoBehaviour
{
    public static InterstitialController instance;
    public static InterstitialController getInstance()
    {
        return instance;
    }

    #if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-2737592620983884/3026211688";
    #elif UNITY_IPHONE
    private string adUnitId = "ca-app-pub-8550386526282187/3705594880";
    #else
    private string adUnitId = "unexpected_platform";
    #endif

    private InterstitialAd interstitial;

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
        RequestInterstitial();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public InterstitialAd GetAd()
    {
        return this.interstitial;
    }

    private void RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }


    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
        this.interstitial.Destroy();
        RequestInterstitial();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        this.interstitial.Destroy();
        RequestInterstitial();
    }
}
