using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AchievementManager : MonoBehaviour
{
    public Animator PopupAnimator;
    public string Name, Description;
    public int Preis;
    public Text NameTxt, DescriptionTxt, PreisTxt;

    public Button NoobBtn, JunkieBtn, SmallStepsBtn, UpgradeBtn, ShoptasticBtn, ShopJunkieBtn, ShareItBtn, WeirdFlexBtn, ThisSucksBtn, RichKidBtn, GodModeBtn, FollowerBtn, TooRichBtn, 
        ShooterGodBtn, CatchGodBtn, HundredBtn;

    public AudioManager SFX;

    private void Start()
    {
        CheckForUnlockedAchievments();
    }
    private void Update()
    {
        if(PopupAnimator.gameObject.activeInHierarchy)
        {
            NameTxt.text = Name;
            DescriptionTxt.text = Description;
            PreisTxt.text = Preis.ToString();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void showAchievement(string name)
    {
        

        switch(name)
        {
            case "Noob":
                {
                    if(PlayerPrefs.GetInt("Noob") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Noob", 1);
                        Name = "Noob";
                        Description = "Reach a Highscore of 0";
                        Preis = 10;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Junkie":
                {
                    if (PlayerPrefs.GetInt("Junkie") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Junkie", 1);
                        Name = "Junkie";
                        Description = "Play for 20 minutes straight";
                        Preis = 30;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Small steps":
                {
                    if (PlayerPrefs.GetInt("Small steps") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Small steps", 1);
                        Name = "Small steps";
                        Description = "Reach a Highscore of 100";
                        Preis = 50;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Upgrade":
                {
                    if (PlayerPrefs.GetInt("Upgrade") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Upgrade", 1);
                        Name = "Upgrade";
                        Description = "Buy an Upgrade from the Shop";
                        Preis = 50;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Shop Tastic":
                {
                    if (PlayerPrefs.GetInt("Shop Tastic") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Shop Tastic", 1);
                        Name = "Shop Tastic";
                        Description = "Buy a Skin from the Shop";
                        Preis = 50;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Shop Junkie":
                {
                    if (PlayerPrefs.GetInt("Shop Junkie") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Shop Junkie", 1);
                        Name = "Shop Junkie";
                        Description = "Buy 5 Skins from the Shop";
                        Preis = 100;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Share It":
                {
                    if (PlayerPrefs.GetInt("Share It") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Share It", 1);
                        Name = "Share It";
                        Description = "Share your Highscore";
                        Preis = 10;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Weird Flex":
                {
                    if (PlayerPrefs.GetInt("Weird Flex") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Weird Flex", 1);
                        Name = "Weird Flex";
                        Description = "Share your Highscore 5 times";
                        Preis = 80;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "This Sucks":
                {
                    if (PlayerPrefs.GetInt("This Sucks") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("This Sucks", 1);
                        Name = "This Sucks";
                        Description = "Sell a Skin in the Shop";
                        Preis = 60;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Rich Kid":
                {
                    if (PlayerPrefs.GetInt("Rich Kid") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Rich Kid", 1);
                        Name = "Rich Kid";
                        Description = "Collect 300 Tricoins";
                        Preis = 50;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "God Mode":
                {
                    if (PlayerPrefs.GetInt("God Mode") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("God Mode", 1);
                        Name = "God Mode";
                        Description = "Reach a Highscore of 300";
                        Preis = 10;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Follower":
                {
                    if (PlayerPrefs.GetInt("Follower") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Follower", 1);
                        Name = "Follower";
                        Description = "Get Tricoins by following us";
                        Preis = 20;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Too Rich??":
                {
                    if (PlayerPrefs.GetInt("Too Rich??") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Too Rich??", 1);
                        Name = "Too Rich??";
                        Description = "Collect 1000 Tricoins";
                        Preis = 500;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Shooter God":
                {
                    if (PlayerPrefs.GetInt("Shooter God") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Shooter God", 1);
                        Name = "Shooter God";
                        Description = "Kill 100 Enemies in Shoot Mode";
                        Preis = 50;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "Catch God":
                {
                    if (PlayerPrefs.GetInt("Catch God") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("Catch God", 1);
                        Name = "Catch God";
                        Description = "Catch 100 Rings in Ring Mode";
                        Preis = 100;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
            case "100%":
                {
                    if (PlayerPrefs.GetInt("100%") == 0)
                    {
                        SFX.Play("Achievement");
                        PlayerPrefs.SetInt("100%", 1);
                        Name = "100%";
                        Description = "Buy all Upgrades and Skins";
                        Preis = 500;
                        PopupAnimator.SetTrigger("SlideIn");
                        Invoke("SlideOut", 2);
                    }
                    return;
                }
        }
    }

    public void SlideOut()
    {
        PopupAnimator.SetTrigger("SlideOut");
    }

    public void CheckForUnlockedAchievments()
    {
        if (PlayerPrefs.GetInt("Noob") == 1)
        {
            NoobBtn.interactable = true;
            NoobBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Junkie") == 1)
        {
            JunkieBtn.interactable = true;
            JunkieBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Small steps") == 1)
        {
            SmallStepsBtn.interactable = true;
            SmallStepsBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Upgrade") == 1)
        {
            UpgradeBtn.interactable = true;
            UpgradeBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Shop Tastic") == 1)
        {
            ShoptasticBtn.interactable = true;
            ShoptasticBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Shop Junkie") == 1)
        {
            ShopJunkieBtn.interactable = true;
            ShopJunkieBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Share It") == 1)
        {
            ShareItBtn.interactable = true;
            ShareItBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Weird Flex") == 1)
        {
            WeirdFlexBtn.interactable = true;
            WeirdFlexBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("This Sucks") == 1)
        {
            ThisSucksBtn.interactable = true;
            ThisSucksBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Rich Kid") == 1)
        {
            RichKidBtn.interactable = true;
            RichKidBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("God Mode") == 1)
        {
            GodModeBtn.interactable = true;
            GodModeBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Follower") == 1)
        {
            FollowerBtn.interactable = true;
            FollowerBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Too Rich??") == 1)
        {
            TooRichBtn.interactable = true;
            TooRichBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Shooter God") == 1)
        {
            ShooterGodBtn.interactable = true;
            ShooterGodBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("Catch God") == 1)
        {
            CatchGodBtn.interactable = true;
            CatchGodBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
        if (PlayerPrefs.GetInt("100%") == 1)
        {
            HundredBtn.interactable = true;
            HundredBtn.gameObject.GetComponent<SpriteHolder>().Unlock();
        }
    }
}
