﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public bool Ostern, Halloween, Weihnachten, Silvester; 
    public PlayerProfile playerProfile;
    public GameObject Player;
    public GameObject gameManager;
    public GameObject AdManager;
    public AchievementManager am;
    public PlayGames pg;
    public Text TricoinsInShop;

    [Header("Buy Buttons Skins")]
    public Button BuyJetButton;
    public Button BuyHeliButton;
    public Button BuyNinjaSternButton;
    public Button BuyMotorradButton;
    public Button BuyEasterButton;
    public Button BuyHalloweenButton;
    public Button BuyChristmasButton;
    public Button BuyNewYearButton;

    [Header("Sell Buttons Skins")]
    public Button SellJetButton;
    public Button SellHeliButton;
    public Button SellNinjaSternButton;
    public Button SellMotorradButton;
    public Button SellEasterButton;
    public Button SellHalloweenButton;
    public Button SellChristmasButton;
    public Button SellNewYearButton;

    [Header("Select Buttons Skins")]
    public Button SelectTriangleButton;
    public Button SelectJetButton;
    public Button SelectHeliButton;
    public Button SelectNinjaSternButton;
    public Button SelectMotorradButton;
    public Button SelectEasterButton;
    public Button SelectHalloweenButton;
    public Button SelectChristmasButton;
    public Button SelectNewYearButton;

    [Header("Select Buttons Background")]
    public Button SelectSolidRedButton;
    public Button SelectSolidGreenButton;
    public Button SelectSolidBlueButton;
    public Button SelectSolidGreyButton;

    private Dictionary<Skin, Button> skinBuyButtons = new Dictionary<Skin, Button>();
    private Dictionary<Skin, Button> skinSelectButtons = new Dictionary<Skin, Button>();
    private Dictionary<Skin, Button> skinSellButtons = new Dictionary<Skin, Button>();
    private Dictionary<Background, Button> backGroundSelectButtons = new Dictionary<Background, Button>();
    private Dictionary<Skin, int> skinPrices = new Dictionary<Skin, int>();

    public GameObject OsterSkin, HalloweenSkin, WeihnachtenSkin, SilvesterSkin;

    void Start()
    {
        playerProfile = SaveManager.Load();

        ShowTricoins();
        // stattdessen:
        Player.GetComponent<FollowFingerScript>().changeSkin(playerProfile.savedCurrentSkin);

        //Hier wird je nach dem welche Skins gekauft wurden, der jeweilige Kauf Button dekativiert, damit der Sell Button aktiv ist und der Select Button des jeweiligen Skins wird aktiv:
        //Wichtig: Ich habe alle Select Buttons in der Szene schon drinnen, die Horizontal Layout group + Content Size fitter sorgen dafür dass sich die aktivierten Select Buttons richtig anordnen

        foreach (Skin s in skinBuyButtons.Keys)
        {
            if (playerProfile.skinPurchased[s])
            {
                skinBuyButtons[s].gameObject.SetActive(false);
                skinSelectButtons[s].gameObject.SetActive(true);
            }
            else
            {
                skinBuyButtons[s].gameObject.SetActive(true);
                skinSelectButtons[s].gameObject.SetActive(false);
            }
        }

        //Preise für die Skins
        skinPrices[Skin.Jet] = 100;
        skinPrices[Skin.Helicopter] = 150;
        skinPrices[Skin.Shuriken] = 300;
        skinPrices[Skin.Motorrad] = 300;
        skinPrices[Skin.Easter] = 300;
        skinPrices[Skin.Halloween] = 300;
        skinPrices[Skin.Christmas] = 300;
        skinPrices[Skin.NewYear] = 300;

        backGroundSelectButtons[Background.SolidRed] = SelectSolidRedButton;
        backGroundSelectButtons[Background.SolidGreen] = SelectSolidGreenButton;
        backGroundSelectButtons[Background.SolidBlue] = SelectSolidBlueButton;
        backGroundSelectButtons[Background.SolidGrey] = SelectSolidGreyButton;

        //Dem Button-Skin Dictionary werden die Skins zugefügt
        #region Skin Buttons
        skinBuyButtons[Skin.Jet] = BuyJetButton;
        skinBuyButtons[Skin.Helicopter] = BuyHeliButton;
        skinBuyButtons[Skin.Shuriken] = BuyNinjaSternButton;
        skinBuyButtons[Skin.Motorrad] = BuyMotorradButton;
        skinBuyButtons[Skin.Easter] = BuyEasterButton;
        skinBuyButtons[Skin.Halloween] = BuyHalloweenButton;
        skinBuyButtons[Skin.Christmas] = BuyChristmasButton;
        skinBuyButtons[Skin.NewYear] = BuyNewYearButton;

        skinSelectButtons[Skin.Triangle] = SelectTriangleButton;
        skinSelectButtons[Skin.Jet] = SelectJetButton;
        skinSelectButtons[Skin.Helicopter] = SelectHeliButton;
        skinSelectButtons[Skin.Shuriken] = SelectNinjaSternButton;
        skinSelectButtons[Skin.Motorrad] = SelectMotorradButton;
        skinSelectButtons[Skin.Easter] = SelectEasterButton;
        skinSelectButtons[Skin.Halloween] = SelectHalloweenButton;
        skinSelectButtons[Skin.Christmas] = SelectChristmasButton;
        skinSelectButtons[Skin.NewYear] = SelectNewYearButton;

        skinSellButtons[Skin.Jet] = SellJetButton;
        skinSellButtons[Skin.Helicopter] = SellHeliButton;
        skinSellButtons[Skin.Shuriken] = SellNinjaSternButton;
        skinSellButtons[Skin.Motorrad] = SellMotorradButton;
        skinSellButtons[Skin.Easter] = SellEasterButton;
        skinSellButtons[Skin.Halloween] = SellHalloweenButton;
        skinSellButtons[Skin.Christmas] = SellChristmasButton;
        skinSellButtons[Skin.NewYear] = SellNewYearButton;

        //Mit dieser Foreach Loop bekommt jeder Background Button seine Function
        foreach (KeyValuePair<Background, Button> bgButtons in backGroundSelectButtons)
        {
            Background val = bgButtons.Key;
            bgButtons.Value.onClick.AddListener(delegate { SelectBG(val); });
        }

        //Mit der foreach Loop bekommt jeder BuyButton Skin den er kaufen soll zugewiesen
        foreach (KeyValuePair<Skin, Button> entry in skinBuyButtons)
        {
            Skin val = entry.Key;
            entry.Value.onClick.AddListener(delegate { BuySkin(val); });
        }
        #endregion

        //Hier werden die Kauf und Verkauf Preise der Buttons zugewiesen
        foreach (Skin s in skinBuyButtons.Keys)
        {
            skinBuyButtons[s].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Buy " + skinPrices[s];
        }

        foreach (Skin s in skinSellButtons.Keys)
        {
            skinSellButtons[s].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Sell " + skinPrices[s] / 2;
        }
        //Select Buttons bekommen die Select Function und als Paramter den passenden Skin

        foreach (KeyValuePair<Skin, Button> entry in skinSelectButtons)
        {
            Skin skin = entry.Key;
            entry.Value.onClick.AddListener(delegate { SelectSkin(skin); });
        }

        foreach (KeyValuePair<Skin, Button> entry in skinSellButtons)
        {
            Skin skin = entry.Key;
            entry.Value.onClick.AddListener(delegate { SellSkin(skin); });
        }

        UpdateSkinCollection();
    }

    public void DeactivateEventSkins()
    {
        if(!Ostern) OsterSkin.SetActive(false);
        if(!Halloween) HalloweenSkin.SetActive(false);
        if(!Weihnachten) WeihnachtenSkin.SetActive(false);
        if(!Silvester) SilvesterSkin.SetActive(false);
    }

    public void checkForBoughtEventSkins()
    {
        if (SelectChristmasButton.gameObject.activeInHierarchy) WeihnachtenSkin.SetActive(true);
        if (SelectEasterButton.gameObject.activeInHierarchy) OsterSkin.SetActive(true);
        if (SelectHalloweenButton.gameObject.activeInHierarchy) HalloweenSkin.SetActive(true);
        if (SelectNewYearButton.gameObject.activeInHierarchy) SilvesterSkin.SetActive(true);
    }

    public void SelectSkin(Skin currentSkin)
    {
        //Hier werden lediglich die Buttons interactable und nicht interactable gemacht, damit sich die Farbe ändert (der selektierte wird grauer und nicht interactable)
        foreach (Skin s in skinSelectButtons.Keys)
        {
            // Gehe durch alle Select Buttons durch und mache die interactable, wenn die nicht zu currentSkin gehören
            if(s == currentSkin) skinSelectButtons[s].interactable = false;

            else skinSelectButtons[s].interactable = true;

            Player.GetComponent<FollowFingerScript>().changeSkin(currentSkin);
            playerProfile.savedCurrentSkin = currentSkin;
            SaveProfile();
        }
    }

    public void UpdateSkinCollection()
    {
        foreach (Skin s in skinBuyButtons.Keys)
        {
            if (playerProfile.skinPurchased[s])
            {
                skinBuyButtons[s].gameObject.SetActive(false);
                skinSelectButtons[s].gameObject.SetActive(true);
            } else
            {
                skinBuyButtons[s].gameObject.SetActive(true);
                skinSelectButtons[s].gameObject.SetActive(false);
            }
        }

        DeactivateEventSkins();
        checkForBoughtEventSkins();
    }

    public void SaveProfile()
    {
        SaveManager.Save();
    }

    public void ShowTricoins()
    {
        TricoinsInShop.text = playerProfile.Tricoins.ToString();
        if (playerProfile.Tricoins >= 500) am.showAchievement("Rich Kid");
        if (playerProfile.Tricoins >= 1000) am.showAchievement("Too Rich??");
    }

    public void BuySkin(Skin skin)
    {
        if (playerProfile.Tricoins >= skinPrices[skin])
        {
            PlayerPrefs.SetInt("Skins", PlayerPrefs.GetInt("Skins", 1) + 1);
            if(PlayerPrefs.GetInt("Skins", 1) >= 5) am.showAchievement("Shop Junkie");

            am.showAchievement("Shop Tastic");
            SelectSkin(skin);
            playerProfile.Tricoins -= skinPrices[skin];
            //Der Bool wird auf true gesetzt, der Skin wird so in dem PlayerProfile gespeichert
            playerProfile.skinPurchased[skin] = true;
            //Der jeweilige KaufButton wird deaktiviert, weil man den Skin ja bereits besitzt
            skinBuyButtons[skin].gameObject.SetActive(false);
            //Es wird nochmal gespeichert
            ShowTricoins();
            SaveProfile();
            UpdateSkinCollection();

        }
    }

    public void SelectBG(Background bg)
    {
        playerProfile.currentBg = bg;
        gameManager.GetComponent<GameManager>().GetBackGroundFromSaveFile();
        SaveProfile();
    }

    public void SellSkin(Skin skin)
    {
        am.showAchievement("This Sucks");
        playerProfile.Tricoins += skinPrices[skin] / 2;
        if (skin == playerProfile.savedCurrentSkin) SelectSkin(Skin.Triangle);
        //Der Bool wird auf false gesetzt, weil der SKin nun nicht mehr im Besitz ist und nicht mehr gespeichert werden soll
        playerProfile.skinPurchased[skin] = false;
        //Der Kauf Button wird wieder aktiv damit man ihn wieder kaufen kann
        skinBuyButtons[skin].gameObject.SetActive(true);
        ShowTricoins();
        SaveProfile();
        UpdateSkinCollection();
    }
}
