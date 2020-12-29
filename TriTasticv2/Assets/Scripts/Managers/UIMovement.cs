using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMovement : MonoBehaviour
{
    //Dieses Skript regelt alle UI Animationen;
    public RectTransform pauseMenu, ShopMenu, StartUPMenu, StartDownMenu, ingameUI, menuUI;
    public GameObject SettingsPanel;
    public GameObject ProfilePanel;
    public GameObject MenuOptionsPanel;
    public GameObject BuyMoreTricoinsPanel;
    public Animator BuyMoreTricoinsPanelAnim;

    void Start()
    {
        //Die Anzahl der Tween die Maximal zurselben Zeit laufen dürfen werden erhöht auf 1250/50
        DOTween.SetTweensCapacity(1250, 50);
    }

    #region startMenuUI

    
    public void MoveStartMenuUIOut()
    {
        StartUPMenu.DOAnchorPos(new Vector2(0,400), 1.5f);
        StartDownMenu.DOAnchorPos(new Vector2(0,-600), 1);
        
        Invoke("MoveInGameUIin", 1);

    }


    //------------------------Settings Menu:--------------------------------------------------------------------------------------------------------
    public void openSettings()
    {
        SettingsPanel.SetActive(true);
        ProfilePanel.SetActive(false);
        MenuOptionsPanel.SetActive(false);
    }
    public void closeSettings()
    {
        SettingsPanel.SetActive(false);
        ProfilePanel.SetActive(false);
        MenuOptionsPanel.SetActive(true);
    }

    //------------------------Menu:--------------------------------------------------------------------------------------------------------
    public void openMenu()
    {
        menuUI.DOAnchorPos(Vector2.zero, 0.5f);
        ProfilePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MenuOptionsPanel.SetActive(true);
    }
    public void closeMenu()
    {
        menuUI.DOAnchorPos(new Vector2(0,1000), 0.5f);
        ProfilePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MenuOptionsPanel.SetActive(true);
    }

    //------------------------Profile Menu:--------------------------------------------------------------------------------------------------------
    public void openProfile()
    {
        ProfilePanel.SetActive(true);
        SettingsPanel.SetActive(false);
        MenuOptionsPanel.SetActive(false);
    }
    public void closeProfile()
    {
        ProfilePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MenuOptionsPanel.SetActive(true);
    }

    //------------------------Shop Menu:--------------------------------------------------------------------------------------------------------
    public void moveShopIn()
    {
        ShopMenu.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void moveShopOut()
    {
        ShopMenu.DOAnchorPos(new Vector2(0, -2500), 1f);
    }

    public void openMoreTricoinsPanel()
    {
        BuyMoreTricoinsPanel.SetActive(true);
    }

    public void closeMoreTricoinsPanel()
    {
        BuyMoreTricoinsPanelAnim.SetTrigger("close");
        Invoke("deactivateMoreTricoinsPanel", 0.8f);
    }

    public void deactivateMoreTricoinsPanel()
    {
        BuyMoreTricoinsPanel.SetActive(false);
    }
    #endregion




    #region ingameUI

    public void MoveInGameUIin()
    {
        ingameUI.DOAnchorPos(new Vector2(0, -50), 1);
        StartUPMenu.gameObject.transform.parent.gameObject.SetActive(false);
    }

    //------------------------Pause Menu:----------------------------------------------------------------------------------------------------------
    public void movePauseMenuIn()
    {
        pauseMenu.DOAnchorPos(Vector2.zero, 0.2f);
    }


    public void movePauseMenuOut()
    {
        pauseMenu.DOAnchorPos(new Vector2(-1000, 0), 0.1f);
    }
    #endregion


}
