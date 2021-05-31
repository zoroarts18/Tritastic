﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public int[] RewardsForDays;
    public Text amount;
    public Text CurrentDayText;
    public PlayerProfile pp;

    public float msToWait = 5000;
    public Button claimRewardBtn;
    public Text RewardTimer;
    public Text RewartTimerTextMenu;
    private ulong lastRewardClaimed;

    public GameObject RewardPanel;
    public GameObject bgPanel;

    public Button openPanelBtn;
    public Button closePanelBtn;

    public static bool RewardPanelShown = false;
    public void Start()
    {
        pp = SaveManager.Load();
        if(!RewardPanelShown)
        {
            RewardPanelShown = true;
            openPanel();
        }
        int currentDay = pp.dailyRewardDayCount + 1;
        CurrentDayText.text = "Day " + currentDay.ToString();
        claimRewardBtn.onClick.AddListener(claimReward);
        lastRewardClaimed = ulong.Parse(PlayerPrefs.GetString("LastRewardClaimed"));

        if (!isRewardReady())
        {
            closePanel();
            claimRewardBtn.interactable = false;
        }

        openPanelBtn.onClick.AddListener(openPanel);
        closePanelBtn.onClick.AddListener(closePanel);
    }

    private void Update()
    {
        amount.text = RewardsForDays[pp.dailyRewardDayCount].ToString() + " Tricoins";
        
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
            RewartTimerTextMenu.text = r;
        }
    }

    public void openPanel()
    {
        RewardPanel.SetActive(true);
        bgPanel.SetActive(true);
    }
    public void closePanel()
    {
        RewardPanel.SetActive(false);
        bgPanel.SetActive(false);
    }

    private bool isRewardReady()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastRewardClaimed);
        ulong ms = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = (float)(msToWait - ms) / 1000.0f;

        if (secondsLeft <= 0)
        {
            RewardTimer.text = "Now!";
            RewartTimerTextMenu.text = "Now!";
            claimRewardBtn.GetComponent<Animator>().SetTrigger("Ready");
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
        claimRewardBtn.GetComponent<Animator>().SetTrigger("Done");
        closePanel();

        if(pp.dailyRewardDayCount <= 4) pp.dailyRewardDayCount++;
        SaveManager.Save();
    }
}
