using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System.Threading;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    private string playStoreID = "3696307";

    private string interstitialAD = "video";
    private string rewardedVideoAd = "rewardedVideo";

    public bool isTargetPlayStore;
    public bool isTestAd;

    public int useOfAD;

    public AudioSource[] audioSources;

    public GameObject ShopManager;
    public GameObject Player;
    public GameObject gameManager;
    public GameObject bgmObject;

    public static int actionId = 0;

    public Button AdToReviveButton;
    public Button AdToGetCoinsButton;

    public static bool isInitialized = false;

    public void Start()
    {
        if(!isInitialized)
        {
            isInitialized = true;
            Advertisement.AddListener(this);
            InitializeAD();
        }

        AdToGetCoinsButton.onClick.AddListener(AdToGetCoins);
        AdToReviveButton.onClick.AddListener(AdToRevive);
    }

    public void InitializeAD()
    {
        Advertisement.Initialize(playStoreID, isTestAd); return; 
    }

    public void AdToGetCoins()
    {
        PlayRewardedAD(1);
    }

    public void AdToRevive()
    {
        PlayRewardedAD(0);
        AdToReviveButton.interactable = false;
    }

    public void PlayRewardedAD(int id)
    {
        if (!Advertisement.IsReady(rewardedVideoAd)) { return; }
        actionId = id;
        Debug.Log("Playing Rewarded Ad");
        Advertisement.Show(rewardedVideoAd);
    }

    public void OnUnityAdsReady(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        GameObject.Find("Background Music").GetComponent<AudioSource>().Pause();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Called OnUnityAdsDidFinish");
        GameObject.Find("Background Music").GetComponent<AudioSource>().UnPause();
        switch(showResult)
        {
            case ShowResult.Failed:
                {
                    return;
                }
            case ShowResult.Skipped:
                {
                    return;
                }
            case ShowResult.Finished:
                {
                    if (actionId == 0)
                    {
                        GameObject.Find("FollowFingerPlayer").GetComponent<FollowFingerScript>().revive();
                    }
                    else if (actionId == 1)
                    {
                        GameObject.Find("ShopManager").GetComponent<Shop>().playerProfile.Tricoins += 20;
                        GameObject.Find("ShopManager").GetComponent<Shop>().ShowTricoins();
                        GameObject.Find("ShopManager").GetComponent<Shop>().SaveProfile();
                    }
                    return;
                }
        }
    }
}
