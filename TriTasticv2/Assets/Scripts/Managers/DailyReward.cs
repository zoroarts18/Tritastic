using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public float msToWait = 5000;
    public Button claimRewardBtn;
    public Text RewardTimer;
    private ulong lastRewardClaimed;

    public GameObject RewardPanel;

    public static bool RewardPanelShown = false;
    public void Start()
    {
        if(!RewardPanelShown)
        {
            RewardPanelShown = true;
            RewardPanel.SetActive(true);
        }

        claimRewardBtn.onClick.AddListener(claimReward);
        lastRewardClaimed = ulong.Parse(PlayerPrefs.GetString("LastRewardClaimed"));

        if (!isRewardReady()) claimRewardBtn.interactable = false;
    }

    private void Update()
    {
        if(!claimRewardBtn.interactable)
        {
            if(isRewardReady())
            {
                claimRewardBtn.interactable = true;
                return;
            }

            //Set Timer:
            ulong diff = ((ulong)DateTime.Now.Ticks - lastRewardClaimed);
            ulong ms = diff / TimeSpan.TicksPerMillisecond;
            float secondsLeft = (float)(msToWait - ms) / 1000.0f;

            string r = "";
            //Hours:
            r += ((int)secondsLeft / 3600).ToString() + "h ";
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            //Minutes:
            r += ((int)secondsLeft/60).ToString("00")+ "m ";
            //Seconds:
            r += (secondsLeft % 60).ToString("00") + "s"; ;

            RewardTimer.text = r;
        }
    }

    private bool isRewardReady()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastRewardClaimed);
        ulong ms = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = (float)(msToWait - ms) / 1000.0f;

        if (secondsLeft <= 0)
        {
            RewardTimer.text = "Now!";
            return true;
        }

        return false;
    }

    public void claimReward()
    {
        lastRewardClaimed = (ulong)DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastRewardClaimed", lastRewardClaimed.ToString());
        claimRewardBtn.interactable = false;
        GameObject.Find("ShopManager").GetComponent<Shop>().playerProfile.Tricoins += 20;
        GameObject.Find("ShopManager").GetComponent<Shop>().ShowTricoins();
        GameObject.Find("ShopManager").GetComponent<Shop>().SaveProfile();

        RewardPanel.SetActive(false);
    }
}
